namespace GV.CounterSketch.Studio.Controls.MarkupConverter;

internal enum HtmlTokenType
{
	OpeningTagStart,
	ClosingTagStart,
	TagEnd,
	EmptyTagEnd,
	EqualSign,
	Name,
	Atom,
	Text,
	Comment,
	EOF
}
