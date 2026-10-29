using System;
using System.IO;
using System.Text;

namespace GV.CounterSketch.Studio.Controls.MarkupConverter;

internal class HtmlLexicalAnalyzer
{
	private readonly StringReader _inputStringReader;

	private int _nextCharacterCode;

	private int _lookAheadCharacterCode;

	private char _lookAheadCharacter;

	private char _previousCharacter;

	private bool _ignoreNextWhitespace;

	private readonly StringBuilder _nextToken;

	internal HtmlTokenType NextTokenType { get; private set; }

	internal string NextToken => _nextToken.ToString();

	private char NextCharacter { get; set; }

	private bool IsAtEndOfStream => _nextCharacterCode == -1;

	private bool IsAtTagStart
	{
		get
		{
			if (NextCharacter == '<' && (_lookAheadCharacter == '/' || IsGoodForNameStart(_lookAheadCharacter)))
			{
				return !IsNextCharacterEntity;
			}
			return false;
		}
	}

	private bool IsAtTagEnd
	{
		get
		{
			if (NextCharacter == '>' || (NextCharacter == '/' && _lookAheadCharacter == '>'))
			{
				return !IsNextCharacterEntity;
			}
			return false;
		}
	}

	private bool IsAtDirectiveStart
	{
		get
		{
			if (NextCharacter == '<' && _lookAheadCharacter == '!')
			{
				return !IsNextCharacterEntity;
			}
			return false;
		}
	}

	private bool IsNextCharacterEntity { get; set; }

	internal HtmlLexicalAnalyzer(string inputTextString)
	{
		_inputStringReader = new StringReader(inputTextString);
		_nextCharacterCode = 0;
		NextCharacter = ' ';
		_lookAheadCharacterCode = _inputStringReader.Read();
		_lookAheadCharacter = (char)_lookAheadCharacterCode;
		_previousCharacter = ' ';
		_ignoreNextWhitespace = true;
		_nextToken = new StringBuilder(100);
		NextTokenType = HtmlTokenType.Text;
		GetNextCharacter();
	}

	internal void GetNextContentToken()
	{
		_nextToken.Length = 0;
		if (IsAtEndOfStream)
		{
			NextTokenType = HtmlTokenType.EOF;
			return;
		}
		if (IsAtTagStart)
		{
			GetNextCharacter();
			if (NextCharacter == '/')
			{
				_nextToken.Append("</");
				NextTokenType = HtmlTokenType.ClosingTagStart;
				GetNextCharacter();
				_ignoreNextWhitespace = false;
			}
			else
			{
				NextTokenType = HtmlTokenType.OpeningTagStart;
				_nextToken.Append("<");
				_ignoreNextWhitespace = true;
			}
			return;
		}
		if (IsAtDirectiveStart)
		{
			GetNextCharacter();
			if (_lookAheadCharacter == '[')
			{
				ReadDynamicContent();
			}
			else if (_lookAheadCharacter == '-')
			{
				ReadComment();
			}
			else
			{
				ReadUnknownDirective();
			}
			return;
		}
		NextTokenType = HtmlTokenType.Text;
		while (!IsAtTagStart && !IsAtEndOfStream && !IsAtDirectiveStart)
		{
			if (NextCharacter == '<' && !IsNextCharacterEntity && _lookAheadCharacter == '?')
			{
				SkipProcessingDirective();
				continue;
			}
			if (NextCharacter <= ' ')
			{
				if (!_ignoreNextWhitespace)
				{
					_nextToken.Append(' ');
				}
				_ignoreNextWhitespace = true;
			}
			else
			{
				_nextToken.Append(NextCharacter);
				_ignoreNextWhitespace = false;
			}
			GetNextCharacter();
		}
	}

	internal void GetNextTagToken()
	{
		_nextToken.Length = 0;
		if (IsAtEndOfStream)
		{
			NextTokenType = HtmlTokenType.EOF;
			return;
		}
		SkipWhiteSpace();
		if (NextCharacter == '>' && !IsNextCharacterEntity)
		{
			NextTokenType = HtmlTokenType.TagEnd;
			_nextToken.Append('>');
			GetNextCharacter();
		}
		else if (NextCharacter == '/' && _lookAheadCharacter == '>')
		{
			NextTokenType = HtmlTokenType.EmptyTagEnd;
			_nextToken.Append("/>");
			GetNextCharacter();
			GetNextCharacter();
			_ignoreNextWhitespace = false;
		}
		else if (IsGoodForNameStart(NextCharacter))
		{
			NextTokenType = HtmlTokenType.Name;
			while (IsGoodForName(NextCharacter) && !IsAtEndOfStream)
			{
				_nextToken.Append(NextCharacter);
				GetNextCharacter();
			}
		}
		else
		{
			NextTokenType = HtmlTokenType.Atom;
			_nextToken.Append(NextCharacter);
			GetNextCharacter();
		}
	}

	internal void GetNextEqualSignToken()
	{
		_nextToken.Length = 0;
		_nextToken.Append('=');
		NextTokenType = HtmlTokenType.EqualSign;
		SkipWhiteSpace();
		if (NextCharacter == '=')
		{
			GetNextCharacter();
		}
	}

	internal void GetNextAtomToken()
	{
		_nextToken.Length = 0;
		SkipWhiteSpace();
		NextTokenType = HtmlTokenType.Atom;
		if ((NextCharacter == '\'' || NextCharacter == '"') && !IsNextCharacterEntity)
		{
			char nextCharacter = NextCharacter;
			GetNextCharacter();
			while ((NextCharacter != nextCharacter || IsNextCharacterEntity) && !IsAtEndOfStream)
			{
				_nextToken.Append(NextCharacter);
				GetNextCharacter();
			}
			if (NextCharacter == nextCharacter)
			{
				GetNextCharacter();
			}
		}
		else
		{
			while (!IsAtEndOfStream && !char.IsWhiteSpace(NextCharacter) && NextCharacter != '>')
			{
				_nextToken.Append(NextCharacter);
				GetNextCharacter();
			}
		}
	}

	private void GetNextCharacter()
	{
		if (_nextCharacterCode == -1)
		{
			throw new InvalidOperationException("GetNextCharacter method called at the end of a stream");
		}
		_previousCharacter = NextCharacter;
		NextCharacter = _lookAheadCharacter;
		_nextCharacterCode = _lookAheadCharacterCode;
		IsNextCharacterEntity = false;
		ReadLookAheadCharacter();
		if (NextCharacter != '&')
		{
			return;
		}
		if (_lookAheadCharacter == '#')
		{
			int num = 0;
			ReadLookAheadCharacter();
			for (int i = 0; i < 7; i++)
			{
				if (!char.IsDigit(_lookAheadCharacter))
				{
					break;
				}
				num = 10 * num + (_lookAheadCharacterCode - 48);
				ReadLookAheadCharacter();
			}
			if (_lookAheadCharacter == ';')
			{
				ReadLookAheadCharacter();
				_nextCharacterCode = num;
				NextCharacter = (char)_nextCharacterCode;
				IsNextCharacterEntity = true;
			}
			else
			{
				NextCharacter = _lookAheadCharacter;
				_nextCharacterCode = _lookAheadCharacterCode;
				ReadLookAheadCharacter();
				IsNextCharacterEntity = false;
			}
		}
		else
		{
			if (!char.IsLetter(_lookAheadCharacter))
			{
				return;
			}
			string text = "";
			for (int j = 0; j < 10; j++)
			{
				if (!char.IsLetter(_lookAheadCharacter) && !char.IsDigit(_lookAheadCharacter))
				{
					break;
				}
				text += _lookAheadCharacter;
				ReadLookAheadCharacter();
			}
			if (_lookAheadCharacter == ';')
			{
				ReadLookAheadCharacter();
				if (HtmlSchema.IsEntity(text))
				{
					NextCharacter = HtmlSchema.EntityCharacterValue(text);
					_nextCharacterCode = NextCharacter;
					IsNextCharacterEntity = true;
				}
				else
				{
					NextCharacter = _lookAheadCharacter;
					_nextCharacterCode = _lookAheadCharacterCode;
					ReadLookAheadCharacter();
					IsNextCharacterEntity = false;
				}
			}
			else
			{
				NextCharacter = _lookAheadCharacter;
				ReadLookAheadCharacter();
				IsNextCharacterEntity = false;
			}
		}
	}

	private void ReadLookAheadCharacter()
	{
		if (_lookAheadCharacterCode != -1)
		{
			_lookAheadCharacterCode = _inputStringReader.Read();
			_lookAheadCharacter = (char)_lookAheadCharacterCode;
		}
	}

	private void SkipWhiteSpace()
	{
		while (true)
		{
			if (NextCharacter == '<' && (_lookAheadCharacter == '?' || _lookAheadCharacter == '!'))
			{
				GetNextCharacter();
				if (_lookAheadCharacter == '[')
				{
					while (!IsAtEndOfStream && (_previousCharacter != ']' || NextCharacter != ']' || _lookAheadCharacter != '>'))
					{
						GetNextCharacter();
					}
					if (NextCharacter == '>')
					{
						GetNextCharacter();
					}
				}
				else
				{
					while (!IsAtEndOfStream && NextCharacter != '>')
					{
						GetNextCharacter();
					}
					if (NextCharacter == '>')
					{
						GetNextCharacter();
					}
				}
			}
			if (char.IsWhiteSpace(NextCharacter))
			{
				GetNextCharacter();
				continue;
			}
			break;
		}
	}

	private bool IsGoodForNameStart(char character)
	{
		if (character != '_')
		{
			return char.IsLetter(character);
		}
		return true;
	}

	private bool IsGoodForName(char character)
	{
		if (!IsGoodForNameStart(character) && character != '.' && character != '-' && character != ':' && !char.IsDigit(character) && !IsCombiningCharacter(character))
		{
			return IsExtender(character);
		}
		return true;
	}

	private bool IsCombiningCharacter(char character)
	{
		return false;
	}

	private bool IsExtender(char character)
	{
		return false;
	}

	private void ReadDynamicContent()
	{
		NextTokenType = HtmlTokenType.Text;
		_nextToken.Length = 0;
		GetNextCharacter();
		GetNextCharacter();
		while ((NextCharacter != ']' || _lookAheadCharacter != '>') && !IsAtEndOfStream)
		{
			GetNextCharacter();
		}
		if (!IsAtEndOfStream)
		{
			GetNextCharacter();
			GetNextCharacter();
		}
	}

	private void ReadComment()
	{
		NextTokenType = HtmlTokenType.Comment;
		_nextToken.Length = 0;
		GetNextCharacter();
		GetNextCharacter();
		GetNextCharacter();
		while (true)
		{
			if (!IsAtEndOfStream && (NextCharacter != '-' || _lookAheadCharacter != '-') && (NextCharacter != '!' || _lookAheadCharacter != '>'))
			{
				_nextToken.Append(NextCharacter);
				GetNextCharacter();
				continue;
			}
			GetNextCharacter();
			if (_previousCharacter == '-' && NextCharacter == '-' && _lookAheadCharacter == '>')
			{
				GetNextCharacter();
				break;
			}
			if (_previousCharacter == '!' && NextCharacter == '>')
			{
				break;
			}
			_nextToken.Append(_previousCharacter);
		}
		if (NextCharacter == '>')
		{
			GetNextCharacter();
		}
	}

	private void ReadUnknownDirective()
	{
		NextTokenType = HtmlTokenType.Text;
		_nextToken.Length = 0;
		GetNextCharacter();
		while ((NextCharacter != '>' || IsNextCharacterEntity) && !IsAtEndOfStream)
		{
			GetNextCharacter();
		}
		if (!IsAtEndOfStream)
		{
			GetNextCharacter();
		}
	}

	private void SkipProcessingDirective()
	{
		GetNextCharacter();
		GetNextCharacter();
		while (((NextCharacter != '?' && NextCharacter != '/') || _lookAheadCharacter != '>') && !IsAtEndOfStream)
		{
			GetNextCharacter();
		}
		if (!IsAtEndOfStream)
		{
			GetNextCharacter();
			GetNextCharacter();
		}
	}
}
