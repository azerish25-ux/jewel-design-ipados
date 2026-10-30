using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using GV.CounterSketch.Shared;
using GV.CounterSketch.Studio.Model.Objects.Enums;
using GV.CounterSketch.Studio.ViewModel;
using Localization;

namespace GV.CounterSketch.Studio.Utilities.Helpers;

public class GemHelper
{
	private static readonly Dictionary<GemMaterial, BitmapImage> _gemMaterialImageCache = new Dictionary<GemMaterial, BitmapImage>();

	public static IEnumerable<MaterialOptionItem> GetGemMaterialOptionItems(bool isListFiltered = false)
	{
		List<MaterialOptionItem> list = new List<MaterialOptionItem>
		{
			new MaterialOptionItem
			{
				IsGem = true,
				GemMaterial = GemMaterial.Diamond,
				IsFavorite = false,
				PrimaryText = "Diamond",
				SecondaryText = " ",
				IsReadOnly = true,
				IsDisableBoxVisible = false,
				ColorGroup = GemColorType.White,
				Description = "Diamond is the birthstone for April, and the gem that marks the 75th wedding anniversary. Originating from the Greek word meaning “unbreakable”, Diamond is the hardest element known to man - the only pure carbon stone. All gem-quality Diamonds are rare, taking billions of years for nature to form them. Fine Diamonds are among the most coveted of all gems."
			},
			new MaterialOptionItem
			{
				IsGem = true,
				GemMaterial = GemMaterial.DiamondKL,
				IsFavorite = false,
				PrimaryText = "Diamond KL",
				SecondaryText = " ",
				IsReadOnly = true,
				ColorGroup = GemColorType.White,
				Description = "Diamond is the birthstone for April and the gem that marks the 75th wedding anniversary. When Diamonds are formed with traces of other minerals, rare and beautiful colors can result. These “fancy” colors range from blue and brilliant yellow to red, brown, pale green, pink and violet. Because of their rarity, colored Diamonds are highly desirable and may be quite valuable."
			},
			new MaterialOptionItem
			{
				IsGem = true,
				GemMaterial = GemMaterial.Moissanite,
				IsFavorite = false,
				PrimaryText = "Moissanite",
				SecondaryText = " ",
				IsReadOnly = true,
				ColorGroup = GemColorType.White,
				Description = "Charles & Colvard Created Moissanite is a favorite among customers. Made from silicon carbide, this unique jewel has more brilliance than a diamond at a fraction of the cost. Created Moissanite is among the hardest minerals on Earth, more so than ruby, emerald, or sapphire."
			},
			new MaterialOptionItem
			{
				IsGem = true,
				GemMaterial = GemMaterial.SapphireWhite,
				IsFavorite = false,
				PrimaryText = "Sapphire White",
				SecondaryText = " ",
				IsReadOnly = true,
				ColorGroup = GemColorType.White,
				Description = "Before cubic zirconia, white sapphire was often used as a substitute for diamonds. Now, white sapphire is coming into its own. Its unique beauty lies in purely reflecting white light."
			},
			new MaterialOptionItem
			{
				IsGem = true,
				GemMaterial = GemMaterial.TopazWhite,
				IsFavorite = false,
				PrimaryText = "Topaz White ",
				SecondaryText = " ",
				IsReadOnly = true,
				ColorGroup = GemColorType.White,
				Description = "White Topaz is often a sensible substitute for Diamonds and a practical choice as the birthstone for April. The cool, calm pureness of White Topaz catches the eye as its inner fire flashes rainbow prisms. The natural magnificence of White Topaz is untreated and readily available in smaller sizes. While White Topaz itself is untreated, its beauty is the foundation for the varieties of treated Topaz available in the market."
			},
			new MaterialOptionItem
			{
				IsGem = true,
				GemMaterial = GemMaterial.CZWhite,
				IsFavorite = false,
				PrimaryText = "CZ White",
				SecondaryText = " ",
				IsReadOnly = true,
				ColorGroup = GemColorType.White,
				Description = "The sparkle of diamonds at an affordable price, Lab-created Cubic Zirconia add drama to a design without the large monetary commitment, freeing you to entertain lots of exciting options."
			},
			new MaterialOptionItem
			{
				IsGem = true,
				GemMaterial = GemMaterial.CZPink,
				IsFavorite = false,
				PrimaryText = "CZ Pink",
				SecondaryText = " ",
				IsReadOnly = true,
				ColorGroup = GemColorType.Pink,
				Description = "A bright pink color with lots of sparkle at an affordable price. Lab-created Cubic Zirconia adds drama to a design without a large monetary commitment, freeing you to entertain lots of exciting options."
			},
			new MaterialOptionItem
			{
				IsGem = true,
				GemMaterial = GemMaterial.CZPinkLight,
				IsFavorite = false,
				PrimaryText = "CZ Pink",
				SecondaryText = "Light",
				IsReadOnly = true,
				ColorGroup = GemColorType.Pink,
				Description = "A light pink color with lots of sparkle at an affordable price. Lab-created Cubic Zirconia adds drama to a design without a large monetary commitment, freeing you to entertain lots of exciting options."
			},
			new MaterialOptionItem
			{
				IsGem = true,
				GemMaterial = GemMaterial.DiamondPink,
				IsFavorite = false,
				PrimaryText = "Diamond Pink",
				SecondaryText = " ",
				IsReadOnly = true,
				ColorGroup = GemColorType.Pink,
				Description = "Diamond is the birthstone for April and the gem that marks the 75th wedding anniversary. When Diamonds are formed with traces of other minerals, rare and beautiful colors can result. These “fancy” colors range from blue and brilliant yellow to red, brown, pale green, pink and violet. Because of their rarity, colored Diamonds are highly desirable and may be quite valuable."
			},
			new MaterialOptionItem
			{
				IsGem = true,
				GemMaterial = GemMaterial.DiamondPinkPure,
				IsFavorite = false,
				PrimaryText = "Diamond Pink",
				SecondaryText = "Pure",
				IsReadOnly = true,
				ColorGroup = GemColorType.Pink,
				Description = "Diamond is the birthstone for April and the gem that marks the 75th wedding anniversary. When Diamonds are formed with traces of other minerals, rare and beautiful colors can result. These “fancy” colors range from blue and brilliant yellow to red, brown, pale green, pink and violet. Because of their rarity, colored Diamonds are highly desirable and may be quite valuable."
			},
			new MaterialOptionItem
			{
				IsGem = true,
				GemMaterial = GemMaterial.GarnetRhodolitePinkRose,
				IsFavorite = false,
				PrimaryText = "Garnet Rhodolite",
				SecondaryText = "Pink Rose",
				IsReadOnly = true,
				ColorGroup = GemColorType.Pink,
				Description = "Garnet is the birthstone for January and the gem for the 2nd  anniversary. Favored since Egyptian times, Garnets appear in stories and legends throughout history."
			},
			new MaterialOptionItem
			{
				IsGem = true,
				GemMaterial = GemMaterial.GarnetRhodoliteRaspberry,
				IsFavorite = false,
				PrimaryText = "Garnet Rhodolite",
				SecondaryText = "Raspberry",
				IsReadOnly = true,
				ColorGroup = GemColorType.Pink,
				Description = "Garnet is the birthstone for January and the gem for the 2nd  anniversary. The velvety-red with tones of raspberry that defines Rhodolite Garnet is highlighted by its impressive brilliance. Favored since Egyptian times, Garnets appear in stories and legends throughout history. Rhodolite Garnet are somewhat lighter in hue than other garnets and have a similar impact to fine rubies, without the accompanying high price."
			},
			new MaterialOptionItem
			{
				IsGem = true,
				GemMaterial = GemMaterial.Morganite,
				IsFavorite = false,
				PrimaryText = "Morganite",
				SecondaryText = " ",
				IsReadOnly = true,
				ColorGroup = GemColorType.Pink,
				Description = "The first gem quality Morganite was discovered in Madagascar and was named in honor of J.P. Morgan, the famous banker and avid mineral collector. Blends of soft pink and orange are typical natural Morganite colors. This highly brilliant stone is ideal for romantic designs that highlight its feminine beauty and soft pink shade."
			},
			new MaterialOptionItem
			{
				IsGem = true,
				GemMaterial = GemMaterial.QuartzRose,
				IsFavorite = false,
				PrimaryText = "Quartz Rose",
				SecondaryText = " ",
				IsReadOnly = true,
				ColorGroup = GemColorType.Pink,
				Description = "Rose Quartz embodies sophistication and elegance. Touched by the palest shade of pink and prized for its subtlety, it has always been a popular and feminine choice. Also called the Heart Stone, Rose Quartz may have been used as a love token as early as 600 B.C. and is still an important talisman of romantic relationships."
			},
			new MaterialOptionItem
			{
				IsGem = true,
				GemMaterial = GemMaterial.SapphirePadparadscha,
				IsFavorite = false,
				PrimaryText = "Sapphire",
				SecondaryText = "Padparadscha",
				IsReadOnly = true,
				ColorGroup = GemColorType.Pink,
				Description = "Blue Sapphire is the birthstone for September and the gem of the 5th and 45th and 70th anniversaries. Depending on their trace element content, Sapphire varieties of the mineral corundum might be blue, yellow, green, orange, pink, purple, white - or even show a six-rayed star if cut as a cabochon."
			},
			new MaterialOptionItem
			{
				IsGem = true,
				GemMaterial = GemMaterial.SapphirePink,
				IsFavorite = false,
				PrimaryText = "Sapphire Pink",
				SecondaryText = " ",
				IsReadOnly = true,
				ColorGroup = GemColorType.Pink,
				Description = "Although the name Sapphire is derived from the Greek meaning “blue,” Sapphires, like the sky can dazzle in many colors. One of the rarest is the beautifully feminine pink variety. Ranging in color from a delicate pastel to a vibrantly tropical pink, this winner is romantic and vibrant, which explains its continuing rise in popularity."
			},
			new MaterialOptionItem
			{
				IsGem = true,
				GemMaterial = GemMaterial.SpinelPink,
				IsFavorite = false,
				PrimaryText = "Spinel Pink",
				SecondaryText = " ",
				IsReadOnly = true,
				ColorGroup = GemColorType.Pink,
				Description = "Often taken for Pink Sapphires, Pink Spinels are a more affordable alternative, even though they are surprisingly more rare. Because most people associate Spinel with the synthetic variety often found in mass-market jewelry, the savvy consumer can enjoy the beauty of this pretty feminine gemstone at a bargain price."
			},
			new MaterialOptionItem
			{
				IsGem = true,
				GemMaterial = GemMaterial.TopazComplexion,
				IsFavorite = false,
				PrimaryText = "Topaz",
				SecondaryText = "Complexion",
				IsReadOnly = true,
				ColorGroup = GemColorType.Pink,
				Description = "Topaz has an exceptionally wide color range that includes various tones and saturations of brown, blue, green, yellow, orange, red, pink, and purple. As the name suggests, complexion topaz is a light peach shade reminiscent of a healthy natural glow."
			},
			new MaterialOptionItem
			{
				IsGem = true,
				GemMaterial = GemMaterial.TopazMysticPurePink,
				IsFavorite = false,
				PrimaryText = "Topaz Mystic",
				SecondaryText = "Pure Pink",
				IsReadOnly = true,
				ColorGroup = GemColorType.Pink,
				Description = "Shimmering and enchanting, Mystic Topaz is a delight to wear. Available in iridescent green, luminous Sunrise or Pure Pink, these affordable and fashionable gems add the ultimate color accent to fashions. As the name suggests, Mystic Topaz is mystical. Its colors are mesmerizing and fun. If you’re looking for affordable, bold color – Mystic Topaz is definitely a gem to satisfy your color cravings."
			},
			new MaterialOptionItem
			{
				IsGem = true,
				GemMaterial = GemMaterial.TopazPassionBabyPink,
				IsFavorite = false,
				PrimaryText = "Topaz Passion",
				SecondaryText = "Baby Pink",
				IsReadOnly = true,
				ColorGroup = GemColorType.Pink,
				Description = "Inspired by the passion for color, the beauty of nature and innovative technology, SWAROVSKI GEMS™ Passion Topaz brings an added touch of something special to fine jewelry. SWAROVSKI GEMS™ Passion Topaz applies an environmentally friendly form of enhancement to genuine White Topaz, creating an iridescent and colorful quality to the stone. The patented treatment is permanent and durable, offering an affordable gem with vast color possibilities. "
			},
			new MaterialOptionItem
			{
				IsGem = true,
				GemMaterial = GemMaterial.TopazPassionPink,
				IsFavorite = false,
				PrimaryText = "Topaz Passion",
				SecondaryText = "Pink",
				IsReadOnly = true,
				ColorGroup = GemColorType.Pink,
				Description = "Inspired by the passion for color, the beauty of nature and innovative technology, SWAROVSKI GEMS™ Passion Topaz brings an added touch of something special to fine jewelry. SWAROVSKI GEMS™ Passion Topaz applies an environmentally friendly form of enhancement to genuine White Topaz, creating an iridescent and colorful quality to the stone. The patented treatment is permanent and durable, offering an affordable gem with vast color possibilities. "
			},
			new MaterialOptionItem
			{
				IsGem = true,
				GemMaterial = GemMaterial.TourmalinePink,
				IsFavorite = false,
				PrimaryText = "Tourmaline",
				SecondaryText = "Pink",
				IsReadOnly = true,
				ColorGroup = GemColorType.Pink,
				Description = "Pink Tourmaline honors the 5th anniversary in addition to sharing in the celebration of 8th anniversaries with its other-color cousins, and October births with Opal. Charmingly pretty, Pink Tourmaline flashes its femininity in a sparkling display. One of the more rare color varieties of Tourmaline, the pink ranges from a pale bubblegum to vibrant, light cherry. The cheerful color so enchanted the last empress of China that she valued it above all gemstones."
			},
			new MaterialOptionItem
			{
				IsGem = true,
				GemMaterial = GemMaterial.DiamondGarnetRed,
				IsFavorite = false,
				PrimaryText = "Diamond Garnet",
				SecondaryText = "Red",
				IsReadOnly = true,
				ColorGroup = GemColorType.Red,
				Description = "Diamond is the birthstone for April and the gem that marks the 75th wedding anniversary. When Diamonds are formed with traces of other minerals, rare and beautiful colors can result. These “fancy” colors range from blue and brilliant yellow to red, brown, pale green, pink and violet. Because of their rarity, colored Diamonds are highly desirable and may be quite valuable."
			},
			new MaterialOptionItem
			{
				IsGem = true,
				GemMaterial = GemMaterial.GarnetBrazilian,
				IsFavorite = false,
				PrimaryText = "Garnet Brazilian",
				SecondaryText = " ",
				IsReadOnly = true,
				ColorGroup = GemColorType.Red,
				Description = "Garnet is the birthstone for January and the gem for the 2nd  anniversary. As its name suggests, this stone is a rich reddish-purple tone. Favored since Egyptian times, Garnets appear in stories and legends throughout history."
			},
			new MaterialOptionItem
			{
				IsGem = true,
				GemMaterial = GemMaterial.GarnetMozambique,
				IsFavorite = false,
				PrimaryText = "Garnet Mozambique",
				SecondaryText = " ",
				IsReadOnly = true,
				ColorGroup = GemColorType.Red,
				Description = "Garnet is the birthstone for January and the gem for the 2nd  anniversary. Garnets appear in stories and legends throughout history. With a warm, rich red color, Mozambique Garnets can set fire to any jewelry design. Not surprisingly, Mozambique is the source for this deep red form of Garnet."
			},
			new MaterialOptionItem
			{
				IsGem = true,
				GemMaterial = GemMaterial.GarnetRhodolite,
				IsFavorite = false,
				PrimaryText = "Garnet Rhodolite",
				SecondaryText = " ",
				IsReadOnly = true,
				ColorGroup = GemColorType.Red,
				Description = "Garnet is the birthstone for January and the gem for the 2nd  anniversary, while Rhodolite also celebrates the 15th anniversary. The velvety-red with tones of raspberry that defines Rhodolite Garnet is highlighted by its impressive brilliance. Rhodolite Garnets are somewhat lighter in hue than other Garnets and have a similar impact to fine Rubies, without the accompanying high price."
			},
			new MaterialOptionItem
			{
				IsGem = true,
				GemMaterial = GemMaterial.OpalMexicanFireRed,
				IsFavorite = false,
				PrimaryText = "Opal Mexican",
				SecondaryText = "Fire Red",
				IsReadOnly = true,
				ColorGroup = GemColorType.Red,
				Description = "One of the most vibrant of gems, it brings a life to jewelry that demands attention. Its blazing reddish-orange color communicates security and success, as well as health and vitality. Treasured by the Maya and Aztecs, Mexico produces this transparent opal and regards it as the national gemstone. Unlike white Opal, Fire Opal is usually faceted."
			},
			new MaterialOptionItem
			{
				IsGem = true,
				GemMaterial = GemMaterial.Ruby,
				IsFavorite = false,
				PrimaryText = "Ruby",
				SecondaryText = " ",
				IsReadOnly = true,
				ColorGroup = GemColorType.Red,
				Description = "Ruby is the birthstone for July and is also the gem for the 15th and 40th anniversaries. Blood-red, fire-red, the color of many fruits when ripe…  Ruby is the color of life itself. It is the most valuable variety of the corundum mineral species, and can command the highest per-carat price of any colored stone. In ancient Sanskrit, Ruby was called ratnaraj, or \"king of precious stones.\""
			},
			new MaterialOptionItem
			{
				IsGem = true,
				GemMaterial = GemMaterial.RubyMadagascar,
				IsFavorite = false,
				PrimaryText = "Ruby Madagascar",
				SecondaryText = " ",
				IsReadOnly = true,
				ColorGroup = GemColorType.Red,
				Description = "Ruby is the birthstone for July and is also the gem for the 15th and 40th anniversaries. Blood-red, fire-red, the color of many fruits when ripe…  Ruby is the color of life itself. It is the most valuable variety of the corundum mineral species, and can command the highest per-carat price of any colored stone. In ancient Sanskrit, Ruby was called ratnaraj, or \"king of precious stones.\""
			},
			new MaterialOptionItem
			{
				IsGem = true,
				GemMaterial = GemMaterial.SapphireRed,
				IsFavorite = false,
				PrimaryText = "Sapphire Red",
				SecondaryText = " ",
				IsReadOnly = true,
				ColorGroup = GemColorType.Red,
				Description = "Depending on their trace element content, Sapphire varieties of the mineral corundum might be blue, yellow, green, red, orange, pink, purple, white - or even show a six-rayed star if cut as a cabochon."
			},
			new MaterialOptionItem
			{
				IsGem = true,
				GemMaterial = GemMaterial.TopazBlazingRed,
				IsFavorite = false,
				PrimaryText = "Topaz Blazing",
				SecondaryText = "Red",
				IsReadOnly = true,
				ColorGroup = GemColorType.Red,
				Description = "Precious Topaz is a birthstone for November, while Imperial Topaz is the gem of the 23rd anniversary. As the name describes, Blazing Red Topaz has a rich, vibrant fiery-red coloration."
			},
			new MaterialOptionItem
			{
				IsGem = true,
				GemMaterial = GemMaterial.Citrine,
				IsFavorite = false,
				PrimaryText = "Citrine",
				SecondaryText = " ",
				IsReadOnly = true,
				ColorGroup = GemColorType.Orange,
				Description = "Citrine is November’s birthstone, alongside Topaz, and the gem for the 11th and  13th anniversary. French for “lemon“, Citrine ranges in color from yellow, light brown to a honey-brown hue. Citrine was once believed to protect against everything from snake venom to wicked thoughts. Today its radiant color is associated with joyfulness, youth and vitality."
			},
			new MaterialOptionItem
			{
				IsGem = true,
				GemMaterial = GemMaterial.CitrineMadeira,
				IsFavorite = false,
				PrimaryText = "Citrine Madeira",
				SecondaryText = " ",
				IsReadOnly = true,
				ColorGroup = GemColorType.Orange,
				Description = "Citrine is a birthstone for November, along with Topaz, and a gem used to celebrate the 11th and 13th anniversaries. Madeira Citrine is suffused with tones of amber, orange and honeyed caramel, and is named after the legendary sherry wines of the Madeira Islands."
			},
			new MaterialOptionItem
			{
				IsGem = true,
				GemMaterial = GemMaterial.DiamondBurntOrange,
				IsFavorite = false,
				PrimaryText = "Diamond Burnt",
				SecondaryText = "Orange",
				IsReadOnly = true,
				ColorGroup = GemColorType.Orange,
				Description = "Diamond is the birthstone for April and the gem that marks the 75th wedding anniversary. When Diamonds are formed with traces of other minerals, rare and beautiful colors can result. These “fancy” colors range from blue and brilliant yellow to red, brown, pale green, pink and violet. Because of their rarity, colored Diamonds are highly desirable and may be quite valuable."
			},
			new MaterialOptionItem
			{
				IsGem = true,
				GemMaterial = GemMaterial.GarnetSpessartite,
				IsFavorite = false,
				PrimaryText = "Garnet Spessartite",
				SecondaryText = " ",
				IsReadOnly = true,
				ColorGroup = GemColorType.Orange,
				Description = "With the orangey colors of a tropical sunset, Spessartite Garnet is a relative newcomer to the jewelry world. Often called Mandarin Garnet, this attractive gemstone was once so rare it was once available only to collectors. "
			},
			new MaterialOptionItem
			{
				IsGem = true,
				GemMaterial = GemMaterial.OpalMexicanFireOrange,
				IsFavorite = false,
				PrimaryText = "Opal Mexican",
				SecondaryText = "Fire Orange",
				IsReadOnly = true,
				ColorGroup = GemColorType.Orange,
				Description = "One of the most vibrant of gems, it brings a life to jewelry that demands attention. Its blazing orange color communicates security and success, as well as health and vitality. Treasured by the Maya and Aztecs, Mexico produces this transparent opal and regards it as the national gemstone. Unlike white Opal, Fire Opal is usually faceted."
			},
			new MaterialOptionItem
			{
				IsGem = true,
				GemMaterial = GemMaterial.SapphireOrange,
				IsFavorite = false,
				PrimaryText = "Sapphire Orange",
				SecondaryText = " ",
				IsReadOnly = true,
				ColorGroup = GemColorType.Orange,
				Description = "Depending on their trace element content, Sapphire varieties of the mineral corundum might be blue, yellow, green, orange, pink, purple, white - or even show a six-rayed star if cut as a cabochon."
			},
			new MaterialOptionItem
			{
				IsGem = true,
				GemMaterial = GemMaterial.TopazMysticSunrise,
				IsFavorite = false,
				PrimaryText = "Topaz Mystic",
				SecondaryText = "Sunrise",
				IsReadOnly = true,
				ColorGroup = GemColorType.Orange,
				Description = "Shimmering and enchanting, Mystic Topaz is a delight to wear. Available in iridescent green, luminous Sunrise or Pure Pink, these affordable and fashionable gems add the ultimate color accent to fashions. As the name suggests, Mystic Topaz is mystical. Its colors are mesmerizing and fun. If you’re looking for affordable, bold color – Mystic Topaz is definitely a gem to satisfy your color cravings."
			},
			new MaterialOptionItem
			{
				IsGem = true,
				GemMaterial = GemMaterial.TopazPoppy,
				IsFavorite = false,
				PrimaryText = "Topaz Poppy",
				SecondaryText = " ",
				IsReadOnly = true,
				ColorGroup = GemColorType.Orange,
				Description = "Inspired by the passion for color, the beauty of nature and innovative technology, SWAROVSKI GEMS™ Passion Topaz brings an added touch of something special to fine jewelry. SWAROVSKI GEMS™ Passion Topaz applies an environmentally friendly form of enhancement to genuine White Topaz, creating an iridescent and colorful quality to the stone. The patented treatment is permanent and durable, offering an affordable gem with vast color possibilities. "
			},
			new MaterialOptionItem
			{
				IsGem = true,
				GemMaterial = GemMaterial.TopazPreciousImperial,
				IsFavorite = false,
				PrimaryText = "Topaz Precious",
				SecondaryText = "Imperial",
				IsReadOnly = true,
				ColorGroup = GemColorType.Orange,
				Description = "Precious Topaz is a birthstone for November, along with Citrine, and Imperial Topaz is the gem of the 23rd anniversary. Topaz has an exceptionally wide color range that includes various tones and saturations of brown, blue, green, yellow, orange, red, pink, and purple."
			},
			new MaterialOptionItem
			{
				IsGem = true,
				GemMaterial = GemMaterial.TourmalineGolden,
				IsFavorite = false,
				PrimaryText = "Tourmaline",
				SecondaryText = "Golden",
				IsReadOnly = true,
				ColorGroup = GemColorType.Orange,
				Description = "Tourmaline is a birthstone for October, along with Opal, and is the gem of the 8th anniversary. Tourmalines have a variety of exciting colors with one of the widest color ranges of any gem, and is known for their hardness and durability."
			},
			new MaterialOptionItem
			{
				IsGem = true,
				GemMaterial = GemMaterial.CitrineYellow,
				IsFavorite = false,
				PrimaryText = "Citrine Yellow",
				SecondaryText = " ",
				IsReadOnly = true,
				ColorGroup = GemColorType.Yellow,
				Description = "Citrine is November’s birthstone, alongside Topaz, and the gem for the 11th and 13th anniversary. French for “lemon“, Citrine ranges in color from yellow, light brown to a honey-brown hue. Citrine was once believed to protect against everything from snake venom to wicked thoughts. Today its radiant color is associated with joyfulness, youth and vitality."
			},
			new MaterialOptionItem
			{
				IsGem = true,
				GemMaterial = GemMaterial.CZYellow,
				IsFavorite = false,
				PrimaryText = "CZ Yellow",
				SecondaryText = " ",
				IsReadOnly = true,
				ColorGroup = GemColorType.Yellow,
				Description = "Bright yellow color with lots of sparkle at an affordable price. Lab-created Cubic Zirconia adds drama to a design without a large monetary commitment, freeing you to entertain lots of exciting options."
			},
			new MaterialOptionItem
			{
				IsGem = true,
				GemMaterial = GemMaterial.DiamondCape,
				IsFavorite = false,
				PrimaryText = "Diamond Cape",
				SecondaryText = " ",
				IsReadOnly = true,
				ColorGroup = GemColorType.Yellow,
				Description = "Diamond is the birthstone for April and the gem that marks the 75th wedding anniversary. When diamonds are formed with traces of other minerals, rare and beautiful colors can result. These “fancy” colors range from blue and brilliant yellow to red, brown, pale green, pink and violet. Because of their rarity, colored diamonds are highly desirable and may be quite valuable."
			},
			new MaterialOptionItem
			{
				IsGem = true,
				GemMaterial = GemMaterial.DiamondYellow,
				IsFavorite = false,
				PrimaryText = "Diamond Yellow",
				SecondaryText = " ",
				IsReadOnly = true,
				ColorGroup = GemColorType.Yellow,
				Description = "Diamond is the birthstone for April and the gem that marks the 75th wedding anniversary. When Diamonds are formed with traces of other minerals, rare and beautiful colors can result. These “fancy” colors range from blue and brilliant yellow to red, brown, pale green, pink and violet. Because of their rarity, colored Diamonds are highly desirable and may be quite valuable."
			},
			new MaterialOptionItem
			{
				IsGem = true,
				GemMaterial = GemMaterial.DiamondYellowGolden,
				IsFavorite = false,
				PrimaryText = "Diamond Yellow",
				SecondaryText = "Golden",
				IsReadOnly = true,
				ColorGroup = GemColorType.Yellow,
				Description = "Diamond is the birthstone for April and the gem that marks the 75th wedding anniversary. When Diamonds are formed with traces of other minerals, rare and beautiful colors can result. These “fancy” colors range from blue and brilliant yellow to red, brown, pale green, pink and violet. Because of their rarity, colored Diamonds are highly desirable and may be quite valuable."
			},
			new MaterialOptionItem
			{
				IsGem = true,
				GemMaterial = GemMaterial.DiamondYellowLight,
				IsFavorite = false,
				PrimaryText = "Diamond Yellow",
				SecondaryText = "Light",
				IsReadOnly = true,
				ColorGroup = GemColorType.Yellow,
				Description = "Diamond is the birthstone for April and the gem that marks the 75th wedding anniversary. When Diamonds are formed with traces of other minerals, rare and beautiful colors can result. These “fancy” colors range from blue and brilliant yellow to red, brown, pale green, pink and violet. Because of their rarity, colored Diamonds are highly desirable and may be quite valuable."
			},
			new MaterialOptionItem
			{
				IsGem = true,
				GemMaterial = GemMaterial.QuartzLemon,
				IsFavorite = false,
				PrimaryText = "Quartz Lemon",
				SecondaryText = " ",
				IsReadOnly = true,
				ColorGroup = GemColorType.Yellow,
				Description = "Lemon Quartz is the clearest expression of sunshine in the Quartz family. Lemon Quartz often displays a subtle green undertone that highlights its sunny nature. An affordable and colorful gemstone, Lemon Quartz is ideal for bold, modern jewelry designs."
			},
			new MaterialOptionItem
			{
				IsGem = true,
				GemMaterial = GemMaterial.SapphireYellow,
				IsFavorite = false,
				PrimaryText = "Sapphire Yellow",
				SecondaryText = " ",
				IsReadOnly = true,
				ColorGroup = GemColorType.Yellow,
				Description = "One of the most popular gems in India and the gem for the 7th anniversary. Yellow Sapphires range from a lovely warm gold to the naturally beautiful canary yellow and is thought to promote wisdom and bring prosperity. Depending on their trace element content, Sapphire varieties of the mineral corundum might be blue, yellow, green, orange, pink, purple, white - or even show a six-rayed star if cut as a cabochon."
			},
			new MaterialOptionItem
			{
				IsGem = true,
				GemMaterial = GemMaterial.TopazPassionHoney,
				IsFavorite = false,
				PrimaryText = "Topaz Passion",
				SecondaryText = "Honey",
				IsReadOnly = true,
				ColorGroup = GemColorType.Yellow,
				Description = "Inspired by the passion for color, the beauty of nature and innovative technology, SWAROVSKI GEMS™ Passion Topaz brings an added touch of something special to fine jewelry. SWAROVSKI GEMS™ Passion Topaz applies an environmentally friendly form of enhancement to genuine White Topaz, creating an iridescent and colorful quality to the stone. The patented treatment is permanent and durable, offering an affordable gem with vast color possibilities."
			},
			new MaterialOptionItem
			{
				IsGem = true,
				GemMaterial = GemMaterial.TopazPreciousGolden,
				IsFavorite = false,
				PrimaryText = "Topaz Precious",
				SecondaryText = "Golden",
				IsReadOnly = true,
				ColorGroup = GemColorType.Yellow,
				Description = "Precious Topaz is a birthstone for November, alongside Citrine, and a gemstone for the 4th and 19th wedding anniversaries. Topaz has an exceptionally wide color range that includes various tones and saturations of brown, blue, green, yellow, orange, red, pink, and purple."
			},
			new MaterialOptionItem
			{
				IsGem = true,
				GemMaterial = GemMaterial.Bloodstone,
				IsFavorite = false,
				PrimaryText = "Bloodstone",
				SecondaryText = " ",
				IsReadOnly = true,
				ColorGroup = GemColorType.Green,
				Description = "The traditional birthstone for March, along with Aquamarine, the combination of profound deep green spangled with vivid spots of red earns this unusual stone the gothically-romantic name of Bloodstone. The startling color contrast has spawned many legends, but today it is valued for its unique aesthetic appeal."
			},
			new MaterialOptionItem
			{
				IsGem = true,
				GemMaterial = GemMaterial.DiamondAppleGreen,
				IsFavorite = false,
				PrimaryText = "Diamond Apple",
				SecondaryText = "Green",
				IsReadOnly = true,
				ColorGroup = GemColorType.Green,
				Description = "Diamond is the birthstone for April and the gem that marks the 75th wedding anniversary. When Diamonds are formed with traces of other minerals, rare and beautiful colors can result. These “fancy” colors range from blue and brilliant yellow to red, brown, pale green, pink and violet. Because of their rarity, colored Diamonds are highly desirable and may be quite valuable."
			},
			new MaterialOptionItem
			{
				IsGem = true,
				GemMaterial = GemMaterial.DiamondDarkGreen,
				IsFavorite = false,
				PrimaryText = "Diamond Dark",
				SecondaryText = "Green",
				IsReadOnly = true,
				ColorGroup = GemColorType.Green,
				Description = "Diamond is the birthstone for April and the gem that marks the 75th wedding anniversary. When Diamonds are formed with traces of other minerals, rare and beautiful colors can result. These “fancy” colors range from blue and brilliant yellow to red, brown, pale green, pink and violet. Because of their rarity, colored Diamonds are highly desirable and may be quite valuable."
			},
			new MaterialOptionItem
			{
				IsGem = true,
				GemMaterial = GemMaterial.Emerald,
				IsFavorite = false,
				PrimaryText = "Emerald",
				SecondaryText = " ",
				IsReadOnly = true,
				ColorGroup = GemColorType.Green,
				Description = "Emerald is the birthstone for May and the gemstone for the 20th, 35th and 55th anniversaries. The source of many legends and mysteries, Emeralds were gathered in Cleopatra's Mines near the Red Sea, worshipped by the Incas and Aztecs, and treasured by the Maharajas of India."
			},
			new MaterialOptionItem
			{
				IsGem = true,
				GemMaterial = GemMaterial.GarnetTsavorite,
				IsFavorite = false,
				PrimaryText = "Garnet Tsavorite",
				SecondaryText = " ",
				IsReadOnly = true,
				ColorGroup = GemColorType.Green,
				Description = "Tsavorite is a beautiful example of how not all Garnets are red. Ranging from a light, verdant green to rich, deep forest, Tsavorite was discovered in 1967 in Tanzania. Enthusiastically promoted by Tiffany & Co., it was Tiffany’s president, Sir Henry Platt, who suggested the name “Ysavorite”, in honor of Tsavo National Park in Kenya."
			},
			new MaterialOptionItem
			{
				IsGem = true,
				GemMaterial = GemMaterial.Peridot,
				IsFavorite = false,
				PrimaryText = "Peridot",
				SecondaryText = " ",
				IsReadOnly = true,
				ColorGroup = GemColorType.Green,
				Description = "Along with Sardonyx, Peridot is the birthstone for August and celebrates the 1st and 16th anniversary. Found in lava, meteorites, and deep in the earth’s mantle, the Hawaiian people believed Peridot to be the tears of the Goddess Pele, and the Egyptians called it the “gem of the sun.” The Romans called it “Evening Emerald” because its brilliant color did not fade in lamp light, but glowed in the evening hours. Peridot was thought to help dreams become reality."
			},
			new MaterialOptionItem
			{
				IsGem = true,
				GemMaterial = GemMaterial.QuartzGreen,
				IsFavorite = false,
				PrimaryText = "Quartz Green",
				SecondaryText = " ",
				IsReadOnly = true,
				ColorGroup = GemColorType.Green,
				Description = "Sometimes referred to as Prasiolite, the color of Green Quartz recalls the new leaves of spring. One of the earliest stones to be venerated, the beauty of Quartz led people to believe that this versatile stone was ice, transformed into an eternal gem by the gods. Green Quartz is a breath of fresh color, verdantly gorgeous at a reasonable price."
			},
			new MaterialOptionItem
			{
				IsGem = true,
				GemMaterial = GemMaterial.TopazMysticGreen,
				IsFavorite = false,
				PrimaryText = "Topaz Mystic",
				SecondaryText = "Green",
				IsReadOnly = true,
				ColorGroup = GemColorType.Green,
				Description = "Shimmering and enchanting, Mystic Topaz is a delight to wear. Available in iridescent green, luminous Sunrise or Pure Pink, these affordable and fashionable gems add the ultimate color accent to fashions. As the name suggests, Mystic Topaz is mystical. Its colors are mesmerizing and fun. If you’re looking for affordable, bold color – Mystic Topaz is definitely a gem to satisfy your color cravings."
			},
			new MaterialOptionItem
			{
				IsGem = true,
				GemMaterial = GemMaterial.TopazRainforest,
				IsFavorite = false,
				PrimaryText = "Topaz Rainforest",
				SecondaryText = " ",
				IsReadOnly = true,
				ColorGroup = GemColorType.Green,
				Description = "Rainforest Topaz gemstones have been enhanced using a standard treatment called \"vacuum deposition\" where a coating of titanium is applied to the pavilion of the gem through the use of electricity, thus giving the stone an ethereal iridescent color, sparkling in lush, tropical shades of the blue-green spectrum."
			},
			new MaterialOptionItem
			{
				IsGem = true,
				GemMaterial = GemMaterial.TourmalineChrome,
				IsFavorite = false,
				PrimaryText = "Tourmaline",
				SecondaryText = "Chrome",
				IsReadOnly = true,
				ColorGroup = GemColorType.Green,
				Description = "Tourmaline is a birthstone for October, along with Opal, and is the gem of the 8th anniversary. Found only in the east African country of Tanzania, Chrome Tourmaline is a rare and wonderful gem. Among the most vivid, intense greens found in the world of jewels, Chrome Tourmaline owes its name to the chromium content that gives this gem its dense, forest green hue."
			},
			new MaterialOptionItem
			{
				IsGem = true,
				GemMaterial = GemMaterial.TourmalineGreen,
				IsFavorite = false,
				PrimaryText = "Tourmaline",
				SecondaryText = "Green",
				IsReadOnly = true,
				ColorGroup = GemColorType.Green,
				Description = "Tourmaline is a birthstone for October, along with Opal, and is the gem of the 8th anniversary. Tourmaline comes in many colors, but for most people the name conjures varying shades of green. Ranging from fresh leek green to a rich intense green - with a multitude of nuances in between - it is highly prized for its verdant palette."
			},
			new MaterialOptionItem
			{
				IsGem = true,
				GemMaterial = GemMaterial.TourmalineOlive,
				IsFavorite = false,
				PrimaryText = "Tourmaline",
				SecondaryText = "Olive",
				IsReadOnly = true,
				ColorGroup = GemColorType.Green,
				Description = "Tourmaline is a birthstone for October, along with Opal, and is the gem of the 8th anniversary. Tourmaline comes in many colors, but for most people the name conjures varying shades of green. Ranging from fresh leek green to a rich intense green - with a multitude of nuances in between - it is highly prized for its verdant palette."
			},
			new MaterialOptionItem
			{
				IsGem = true,
				GemMaterial = GemMaterial.TourmalineParaiba,
				IsFavorite = false,
				PrimaryText = "Tourmaline",
				SecondaryText = "Paraiba",
				IsReadOnly = true,
				ColorGroup = GemColorType.Green,
				Description = "Tourmaline is a birthstone for October, along with Opal, and is the gem of the 8th anniversary. Tourmaline comes in many colors, but for most people the name conjures varying shades of green. Ranging from fresh leek green to a rich intense green - with a multitude of nuances in between - it is highly prized for its verdant palette."
			},
			new MaterialOptionItem
			{
				IsGem = true,
				GemMaterial = GemMaterial.Aquamarine,
				IsFavorite = false,
				PrimaryText = "Aquamarine",
				SecondaryText = " ",
				IsReadOnly = true,
				ColorGroup = GemColorType.Aqua,
				Description = "Aquamarine is the birthstone for March, along with Bloodstone, and is the gemstone for the 16th and 19th anniversaries. With color reminiscent of tropical oceans, it’s no surprise this gemstone’s name derives from Latin for “water of the sea.” The Romans believed Aquamarine absorbed and protected young love, making it an ideal romantic gift."
			},
			new MaterialOptionItem
			{
				IsGem = true,
				GemMaterial = GemMaterial.DiamondAquaBlue,
				IsFavorite = false,
				PrimaryText = "Diamond Aqua",
				SecondaryText = "Blue",
				IsReadOnly = true,
				ColorGroup = GemColorType.Aqua,
				Description = "Diamond is the birthstone for April and the gem that marks the 75th wedding anniversary. When Diamonds are formed with traces of other minerals, rare and beautiful colors can result. These “fancy” colors range from blue and brilliant yellow to red, brown, pale green, pink and violet. Because of their rarity, colored Diamonds are highly desirable and may be quite valuable."
			},
			new MaterialOptionItem
			{
				IsGem = true,
				GemMaterial = GemMaterial.DiamondTealBlue,
				IsFavorite = false,
				PrimaryText = "Diamond Teal",
				SecondaryText = "Blue",
				IsReadOnly = true,
				ColorGroup = GemColorType.Aqua,
				Description = "Diamond is the birthstone for April and the gem that marks the 75th wedding anniversary. When Diamonds are formed with traces of other minerals, rare and beautiful colors can result. These “fancy” colors range from blue and brilliant yellow to red, brown, pale green, pink and violet. Because of their rarity, colored Diamonds are highly desirable and may be quite valuable."
			},
			new MaterialOptionItem
			{
				IsGem = true,
				GemMaterial = GemMaterial.SpinelAqua,
				IsFavorite = false,
				PrimaryText = "Spinel Aqua",
				SecondaryText = " ",
				IsReadOnly = true,
				ColorGroup = GemColorType.Aqua,
				Description = "Although rare, Spinels are an affordable gemstone that mark the 65th anniversary. Because most people associate Spinel with the synthetic variety often found in mass-market jewelry, the savvy consumer can enjoy the beauty of this pretty feminine gemstone at a bargain price."
			},
			new MaterialOptionItem
			{
				IsGem = true,
				GemMaterial = GemMaterial.TopazAquaBlue,
				IsFavorite = false,
				PrimaryText = "Topaz Aqua",
				SecondaryText = "Blue",
				IsReadOnly = true,
				ColorGroup = GemColorType.Aqua,
				Description = "Blue Topaz is a birthstone for December, along with Turquoise, Tanzanite, and Blue Zircon, and is the gem of the 4th anniversary.  From the pure pastel blue of an alpine glacier, the glow of the mid-summer sky, and the aquatic hue of the deepest ocean, refreshing Blue Topaz brings a breath of fresh air to contemporary jewelry designs. Blue Topaz rivals Aquamarine in color and charm. Found in greater abundance, it offers superb clarity and luminous color at a fraction of the cost."
			},
			new MaterialOptionItem
			{
				IsGem = true,
				GemMaterial = GemMaterial.TopazIceBlue,
				IsFavorite = false,
				PrimaryText = "Topaz Ice",
				SecondaryText = "Blue",
				IsReadOnly = true,
				ColorGroup = GemColorType.Aqua,
				Description = "Blue topaz is a birthstone for December, the gem of the 4th anniversary and the state gemstone of Texas.  From the pure pastel blue of an Alpine glacier, the glow of the mid-summer sky, and the aquatic hue of the deepest ocean, refreshing Blue Topaz brings a breath of fresh air to contemporary jewelry designs. Blue Topaz rivals Aquamarine in color and charm. Found in greater abundance, it offers superb clarity and luminous color at a fraction of the cost."
			},
			new MaterialOptionItem
			{
				IsGem = true,
				GemMaterial = GemMaterial.TopazIceBlueFrosted,
				IsFavorite = false,
				PrimaryText = "Topaz Ice",
				SecondaryText = "Blue Frosted",
				IsReadOnly = true,
				ColorGroup = GemColorType.Aqua,
				Description = "Blue topaz is a birthstone for December, the gem of the 4th anniversary and the state gemstone of Texas.  From the pure pastel blue of an Alpine glacier, the glow of the mid-summer sky, and the aquatic hue of the deepest ocean, refreshing Blue Topaz brings a breath of fresh air to contemporary jewelry designs. Blue Topaz rivals Aquamarine in color and charm. Found in greater abundance, it offers superb clarity and luminous color at a fraction of the cost."
			},
			new MaterialOptionItem
			{
				IsGem = true,
				GemMaterial = GemMaterial.TopazParadiseBlue,
				IsFavorite = false,
				PrimaryText = "Topaz Paradise",
				SecondaryText = "Blue",
				IsReadOnly = true,
				ColorGroup = GemColorType.Aqua,
				Description = "Blue topaz is a birthstone for December, the gem of the 4th anniversary and the state gemstone of Texas.  From the pure pastel blue of an Alpine glacier, the glow of the mid-summer sky, and the aquatic hue of the deepest ocean, refreshing Blue Topaz brings a breath of fresh air to contemporary jewelry designs. Blue Topaz rivals Aquamarine in color and charm. Found in greater abundance, it offers superb clarity and luminous color at a fraction of the cost."
			},
			new MaterialOptionItem
			{
				IsGem = true,
				GemMaterial = GemMaterial.TopazParaiba,
				IsFavorite = false,
				PrimaryText = "Topaz Paraiba",
				SecondaryText = " ",
				IsReadOnly = true,
				ColorGroup = GemColorType.Aqua,
				Description = "Inspired by the passion for color, the beauty of nature and innovative technology, SWAROVSKI GEMS™ Passion Topaz brings an added touch of something special to fine jewelry. SWAROVSKI GEMS™ Passion Topaz applies an environmentally friendly form of enhancement to genuine White Topaz, creating an iridescent and colorful quality to the stone. The patented treatment is permanent and durable, offering an affordable gem with vast color possibilities."
			},
			new MaterialOptionItem
			{
				IsGem = true,
				GemMaterial = GemMaterial.TopazPassionAquaBlue,
				IsFavorite = false,
				PrimaryText = "Topaz Passion",
				SecondaryText = "Aqua Blue",
				IsReadOnly = true,
				ColorGroup = GemColorType.Aqua,
				Description = "Inspired by the passion for color, the beauty of nature and innovative technology, SWAROVSKI GEMS™ Passion Topaz brings an added touch of something special to fine jewelry. SWAROVSKI GEMS™ Passion Topaz applies an environmentally friendly form of enhancement to genuine White Topaz, creating an iridescent and colorful quality to the stone. The patented treatment is permanent and durable, offering an affordable gem with vast color possibilities. "
			},
			new MaterialOptionItem
			{
				IsGem = true,
				GemMaterial = GemMaterial.TopazSkyBlue,
				IsFavorite = false,
				PrimaryText = "Topaz Sky",
				SecondaryText = "Blue",
				IsReadOnly = true,
				ColorGroup = GemColorType.Aqua,
				Description = "Blue Topaz is a birthstone for December, along with Turquoise, Tanzanite, and Blue Zircon, and is the gem of the 4th anniversary.  From the pure pastel blue of an alpine glacier, the glow of the mid-summer sky, and the aquatic hue of the deepest ocean, refreshing Blue Topaz brings a breath of fresh air to contemporary jewelry designs."
			},
			new MaterialOptionItem
			{
				IsGem = true,
				GemMaterial = GemMaterial.TopazTeal,
				IsFavorite = false,
				PrimaryText = "Topaz Teal",
				SecondaryText = " ",
				IsReadOnly = true,
				ColorGroup = GemColorType.Aqua,
				Description = "Blue Topaz is a birthstone for December, the gem of the 4th anniversary.  From the pure pastel blue of an Alpine glacier, the glow of the mid-summer sky, and the aquatic hue of the deepest ocean, refreshing Blue Topaz brings a breath of fresh air to contemporary jewelry designs. Blue Topaz rivals Aquamarine in color and charm. Found in greater abundance, it offers superb clarity and luminous color at a fraction of the cost."
			},
			new MaterialOptionItem
			{
				IsGem = true,
				GemMaterial = GemMaterial.TopazTurquoiseFrosted,
				IsFavorite = false,
				PrimaryText = "Topaz Turquoise",
				SecondaryText = "Frosted",
				IsReadOnly = true,
				ColorGroup = GemColorType.Aqua,
				Description = "Blue Topaz is a birthstone for December, the gem of the 4th anniversary.  From the pure pastel blue of an Alpine glacier, the glow of the mid-summer sky, and the aquatic hue of the deepest ocean, refreshing Blue Topaz brings a breath of fresh air to contemporary jewelry designs. Found in greater abundance, it offers superb clarity and luminous color at a fraction of the cost."
			},
			new MaterialOptionItem
			{
				IsGem = true,
				GemMaterial = GemMaterial.TourmalineBlueParaiba,
				IsFavorite = false,
				PrimaryText = "Tourmaline",
				SecondaryText = "Blue Paraiba",
				IsReadOnly = true,
				ColorGroup = GemColorType.Aqua,
				Description = "Tourmaline is a birthstone for October, along with Opal, and is the gem of the 8th anniversary.  Tourmaline comes in a variety of exciting colors, with one of the widest color ranges of any gem, and is known for hardness and durability. Paraíba is an intense violetish-blue, greenish-blue, or blue Tourmaline originating from the state of Paraíba, Brazil."
			},
			new MaterialOptionItem
			{
				IsGem = true,
				GemMaterial = GemMaterial.ZirconBlue,
				IsFavorite = false,
				PrimaryText = "Zircon Blue",
				SecondaryText = " ",
				IsReadOnly = true,
				ColorGroup = GemColorType.Aqua,
				Description = "Along with Turquoise, Blue Topaz and Tanzanite, Blue Zircon is also a birthstone for the month of December and is often used to mark 4th anniversaries. An enchanting shade of sea blue, this exceptional jewel shines brightly among gems and blue is the most popular color. In fact, so brilliant and fiery is Blue Zircon that it rivals the far more costly Blue Diamond."
			},
			new MaterialOptionItem
			{
				IsGem = true,
				GemMaterial = GemMaterial.Alexandrite,
				IsFavorite = false,
				PrimaryText = "Alexandrite",
				SecondaryText = " ",
				IsReadOnly = true,
				ColorGroup = GemColorType.Blue,
				Description = "Alexandrite is often cited as a birthstone for June, along with both Pearl and Moonstone, and is the gemstone of the 45th and 55th wedding anniversaries. This fascinating gemstone’s color changes depending on the light source: from bluish-green in daylight, to purplish-red in incandescent."
			},
			new MaterialOptionItem
			{
				IsGem = true,
				GemMaterial = GemMaterial.DiamondBlue,
				IsFavorite = false,
				PrimaryText = "Diamond Blue",
				SecondaryText = " ",
				IsReadOnly = true,
				ColorGroup = GemColorType.Blue,
				Description = "Diamond is the birthstone for April and the gem that marks the 75th wedding anniversary. When Diamonds are formed with traces of other minerals, rare and beautiful colors can result. These “fancy” colors range from blue and brilliant yellow to red, brown, pale green, pink and violet. Because of their rarity, colored Diamonds are highly desirable and may be quite valuable."
			},
			new MaterialOptionItem
			{
				IsGem = true,
				GemMaterial = GemMaterial.Iolite,
				IsFavorite = false,
				PrimaryText = "Iolite",
				SecondaryText = " ",
				IsReadOnly = true,
				ColorGroup = GemColorType.Blue,
				Description = "The stone of the 21st anniversary, Iolite’s name was derived from the Greek word for “violet”. An interesting property of Iolite is its color change with the direction of the light that illuminates it. Because of this property, the Vikings used Iolite on their voyages to determine the direction of the sun in dense fog, overcast conditions and even when the sun was below the horizon, giving it the name Vikings’ Compass."
			},
			new MaterialOptionItem
			{
				IsGem = true,
				GemMaterial = GemMaterial.SapphireBlue,
				IsFavorite = false,
				PrimaryText = "Sapphire Blue",
				SecondaryText = " ",
				IsReadOnly = true,
				ColorGroup = GemColorType.Blue,
				Description = "Blue Sapphire is the birthstone for September and the gem of the 5th, 10th and 45th anniversaries. The breathtaking blue of Sapphire has captured man’s imagination for millennia. The ancients believed that Blue Sapphire had the ability to make peace between warring parties. An enduring symbol for loyalty and trust, it’s frequently chosen to celebrate engagements or anniversaries."
			},
			new MaterialOptionItem
			{
				IsGem = true,
				GemMaterial = GemMaterial.TopazGlacierBlue,
				IsFavorite = false,
				PrimaryText = "Topaz Glacier",
				SecondaryText = "Blue",
				IsReadOnly = true,
				ColorGroup = GemColorType.Blue,
				Description = "Blue topaz is a birthstone for December, the gem of the 4th anniversary and the state gemstone of Texas.  From the pure pastel blue of an Alpine glacier, the glow of the mid-summer sky, and the aquatic hue of the deepest ocean, refreshing Blue Topaz brings a breath of fresh air to contemporary jewelry designs. Blue Topaz rivals Aquamarine in color and charm. Found in greater abundance, it offers superb clarity and luminous color at a fraction of the cost."
			},
			new MaterialOptionItem
			{
				IsGem = true,
				GemMaterial = GemMaterial.TopazLondonBlue,
				IsFavorite = false,
				PrimaryText = "Topaz London",
				SecondaryText = "Blue",
				IsReadOnly = true,
				ColorGroup = GemColorType.Blue,
				Description = "Blue topaz is a birthstone for December, the gem of the 4th anniversary and the state gemstone of Texas.  From the pure pastel blue of an Alpine glacier, the glow of the mid-summer sky, and the aquatic hue of the deepest ocean, refreshing Blue Topaz brings a breath of fresh air to contemporary jewelry designs. Blue Topaz rivals Aquamarine in color and charm. Found in greater abundance, it offers superb clarity and luminous color at a fraction of the cost."
			},
			new MaterialOptionItem
			{
				IsGem = true,
				GemMaterial = GemMaterial.TopazPassionGlacierBlue,
				IsFavorite = false,
				PrimaryText = "Topaz Passion",
				SecondaryText = "Glacier Blue",
				IsReadOnly = true,
				ColorGroup = GemColorType.Blue,
				Description = "Inspired by the passion for color, the beauty of nature and innovative technology, SWAROVSKI GEMS™ Passion Topaz brings an added touch of something special to fine jewelry. SWAROVSKI GEMS™ Passion Topaz applies an environmentally friendly form of enhancement to genuine White Topaz, creating an iridescent and colorful quality to the stone. The patented treatment is permanent and durable, offering an affordable gem with vast color possibilities."
			},
			new MaterialOptionItem
			{
				IsGem = true,
				GemMaterial = GemMaterial.TopazPassionIceBlue,
				IsFavorite = false,
				PrimaryText = "Topaz Passion",
				SecondaryText = "Ice Blue",
				IsReadOnly = true,
				ColorGroup = GemColorType.Blue,
				Description = "Inspired by the passion for color, the beauty of nature and innovative technology, SWAROVSKI GEMS™ Passion Topaz brings an added touch of something special to fine jewelry. SWAROVSKI GEMS™ Passion Topaz applies an environmentally friendly form of enhancement to genuine White Topaz, creating an iridescent and colorful quality to the stone. The patented treatment is permanent and durable, offering an affordable gem with vast color possibilities."
			},
			new MaterialOptionItem
			{
				IsGem = true,
				GemMaterial = GemMaterial.TopazPassionParadiseBlue,
				IsFavorite = false,
				PrimaryText = "Topaz Passion",
				SecondaryText = "Paradise Blue",
				IsReadOnly = true,
				ColorGroup = GemColorType.Blue,
				Description = "Inspired by the passion for color, the beauty of nature and innovative technology, SWAROVSKI GEMS™ Passion Topaz brings an added touch of something special to fine jewelry. SWAROVSKI GEMS™ Passion Topaz applies an environmentally friendly form of enhancement to genuine White Topaz, creating an iridescent and colorful quality to the stone. The patented treatment is permanent and durable, offering an affordable gem with vast color possibilities."
			},
			new MaterialOptionItem
			{
				IsGem = true,
				GemMaterial = GemMaterial.TopazPassionTeal,
				IsFavorite = false,
				PrimaryText = "Topaz Passion",
				SecondaryText = "Teal",
				IsReadOnly = true,
				ColorGroup = GemColorType.Blue,
				Description = "Inspired by the passion for color, the beauty of nature and innovative technology, SWAROVSKI GEMS™ Passion Topaz brings an added touch of something special to fine jewelry. SWAROVSKI GEMS™ Passion Topaz applies an environmentally friendly form of enhancement to genuine White Topaz, creating an iridescent and colorful quality to the stone. The patented treatment is permanent and durable, offering an affordable gem with vast color possibilities."
			},
			new MaterialOptionItem
			{
				IsGem = true,
				GemMaterial = GemMaterial.TopazSwissBlue,
				IsFavorite = false,
				PrimaryText = "Topaz Swiss",
				SecondaryText = "Blue",
				IsReadOnly = true,
				ColorGroup = GemColorType.Blue,
				Description = "Blue Topaz is a birthstone for December, the gem of the 4th anniversary.  From the pure pastel blue of an Alpine glacier, the glow of the mid-summer sky, and the aquatic hue of the deepest ocean, refreshing Blue Topaz brings a breath of fresh air to contemporary jewelry designs. Blue Topaz rivals Aquamarine in color and charm. Found in greater abundance, it offers superb clarity and luminous color at a fraction of the cost."
			},
			new MaterialOptionItem
			{
				IsGem = true,
				GemMaterial = GemMaterial.Amethyst,
				IsFavorite = false,
				PrimaryText = "Amethyst",
				SecondaryText = " ",
				IsReadOnly = true,
				ColorGroup = GemColorType.Purple,
				Description = "The birthstone for February and the gem to celebrate the 4th, 6th, 8th and 9th, Amethyst ranges in color from a delicate lilac to deepest plum. This regal gemstone has been valued for millennia. Believed to promote sobriety and clarity of thought, this form of Quartz was used in the Middle Ages as an antidote to poison."
			},
			new MaterialOptionItem
			{
				IsGem = true,
				GemMaterial = GemMaterial.CZLavender,
				IsFavorite = false,
				PrimaryText = "CZ Lavender",
				SecondaryText = " ",
				IsReadOnly = true,
				ColorGroup = GemColorType.Purple,
				Description = "A vivid lilac color with lots of sparkle at an affordable price. Lab-created Cubic Zirconia adds drama to a design without a large monetary commitment, freeing you to entertain lots of exciting options."
			},
			new MaterialOptionItem
			{
				IsGem = true,
				GemMaterial = GemMaterial.DiamondPurple,
				IsFavorite = false,
				PrimaryText = "Diamond Purple",
				SecondaryText = " ",
				IsReadOnly = true,
				ColorGroup = GemColorType.Purple,
				Description = "Diamond is the birthstone for April and the gem that marks the 75th wedding anniversary. When Diamonds are formed with traces of other minerals, rare and beautiful colors can result. These “fancy” colors range from blue and brilliant yellow to red, brown, pale green, pink and violet. Because of their rarity, colored Diamonds are highly desirable and may be quite valuable."
			},
			new MaterialOptionItem
			{
				IsGem = true,
				GemMaterial = GemMaterial.GarnetColorChange,
				IsFavorite = false,
				PrimaryText = "Garnet",
				SecondaryText = "Color Change",
				IsReadOnly = true,
				ColorGroup = GemColorType.Purple,
				Description = "Garnet is the birthstone for January and the gem for the 2nd anniversary. Color-change Garnet is a mix of Spessartite and Pyrope Garnet. This Garnet presents a color change from bluish-green in daylight to a reddish-purple in incandescent light."
			},
			new MaterialOptionItem
			{
				IsGem = true,
				GemMaterial = GemMaterial.QuartzRoseDeFrance,
				IsFavorite = false,
				PrimaryText = "Quartz Rose",
				SecondaryText = "De France",
				IsReadOnly = true,
				ColorGroup = GemColorType.Purple,
				Description = "Rose de France Quartz is the lightest shade of amethyst - lingering in the pale lavender to lilac range. Less costly than its darker Amethyst cousins, it has recently been vaulted to popularity by fashion colors and trends. Rose de France Quartz works well whether in earrings, rings, bracelets, or as a pendant."
			},
			new MaterialOptionItem
			{
				IsGem = true,
				GemMaterial = GemMaterial.SapphirePurple,
				IsFavorite = false,
				PrimaryText = "Sapphire Purple",
				SecondaryText = " ",
				IsReadOnly = true,
				ColorGroup = GemColorType.Purple,
				Description = "Description"
			},
			new MaterialOptionItem
			{
				IsGem = true,
				GemMaterial = GemMaterial.SapphireViolet,
				IsFavorite = false,
				PrimaryText = "Sapphire Violet",
				SecondaryText = " ",
				IsReadOnly = true,
				ColorGroup = GemColorType.Purple,
				Description = "Blue Sapphire is the birthstone for September and the gem of the 5th and 45th and 70th anniversaries. Depending on their trace element content, Sapphire varieties of the mineral corundum might be blue, yellow, green, orange, pink, purple, white - or even show a six-rayed star if cut as a cabochon."
			},
			new MaterialOptionItem
			{
				IsGem = true,
				GemMaterial = GemMaterial.Tanzanite,
				IsFavorite = false,
				PrimaryText = "Tanzanite",
				SecondaryText = " ",
				IsReadOnly = true,
				ColorGroup = GemColorType.Purple,
				Description = "Tanzanite is a birthstone for December, along with Turquoise, Blue Topaz and Blue Zircon, and the gemstone of the 8th and 24th anniversaries. Tanzanite’s rarity and exotic origin have added to its fascination. This modern gemstone was unknown until 1967, when Masai herdsmen in eastern Africa noticed blue crystals sparkling in the sun. Tanzanite’s dramatic discovery, coupled with its scintillating beauty, caused a worldwide sensation. To date, the world’s only source for the gem remains the hills of northern Tanzania, near Mount Kilimanjaro."
			},
			new MaterialOptionItem
			{
				IsGem = true,
				GemMaterial = GemMaterial.DiamondChampagne,
				IsFavorite = false,
				PrimaryText = "Diamond Champagne",
				SecondaryText = " ",
				IsReadOnly = true,
				ColorGroup = GemColorType.Brown,
				Description = "Diamond is the birthstone for April and the gem that marks the 75th wedding anniversary. When Diamonds are formed with traces of other minerals, rare and beautiful colors can result. These “fancy” colors range from blue and brilliant yellow to red, brown, pale green, pink and violet. Because of their rarity, colored Diamonds are highly desirable and may be quite valuable."
			},
			new MaterialOptionItem
			{
				IsGem = true,
				GemMaterial = GemMaterial.DiamondCognac,
				IsFavorite = false,
				PrimaryText = "Diamond Cognac",
				SecondaryText = " ",
				IsReadOnly = true,
				ColorGroup = GemColorType.Brown,
				Description = "Diamond is the birthstone for April and the gem that marks the 75th wedding anniversary. When Diamonds are formed with traces of other minerals, rare and beautiful colors can result. These “fancy” colors range from blue and brilliant yellow to red, brown, pale green, pink and violet. Because of their rarity, colored Diamonds are highly desirable and may be quite valuable."
			},
			new MaterialOptionItem
			{
				IsGem = true,
				GemMaterial = GemMaterial.DiamondTopLightBrown,
				IsFavorite = false,
				PrimaryText = "Diamond Top",
				SecondaryText = "Light Brown",
				IsReadOnly = true,
				ColorGroup = GemColorType.Brown,
				Description = "Diamond is the birthstone for April and the gem that marks the 75th wedding anniversary. When Diamonds are formed with traces of other minerals, rare and beautiful colors can result. These “fancy” colors range from blue and brilliant yellow to red, brown, pale green, pink and violet. Because of their rarity, colored Diamonds are highly desirable and may be quite valuable."
			},
			new MaterialOptionItem
			{
				IsGem = true,
				GemMaterial = GemMaterial.GarnetAndradite,
				IsFavorite = false,
				PrimaryText = "Garnet Andradite",
				SecondaryText = " ",
				IsReadOnly = true,
				ColorGroup = GemColorType.Brown,
				Description = "Andradite Garnets range in a wide spectrum of earthy autumn-inspired colors such as rich browns to moss green and shades in between. Favored since Egyptian times, Garnets appear in stories and legends throughout history."
			},
			new MaterialOptionItem
			{
				IsGem = true,
				GemMaterial = GemMaterial.QuartzSmoky,
				IsFavorite = false,
				PrimaryText = "Quartz Smoky",
				SecondaryText = " ",
				IsReadOnly = true,
				ColorGroup = GemColorType.Brown,
				Description = "Smoky Quartz represents the 70th anniversary. Subtlety is the appeal of this lustrous stone. With its voluptuous autumn color and understated elegance, this sophisticated gemstone makes a powerful statement. The Victorians, who understood the rich expressiveness of restraint, used Smoky Quartz frequently in jewelry. The delicious color of Smoky Quartz makes it a perfect accessory for all seasons."
			},
			new MaterialOptionItem
			{
				IsGem = true,
				GemMaterial = GemMaterial.TourmalineBronze,
				IsFavorite = false,
				PrimaryText = "Tourmaline",
				SecondaryText = "Bronze",
				IsReadOnly = true,
				ColorGroup = GemColorType.Brown,
				Description = "Tourmaline is a birthstone for October, along with Opal, and is the gem of the 8th anniversary. Tourmalines have a variety of exciting colors with one of the widest color ranges of any gem, and is known for their hardness and durability."
			},
			new MaterialOptionItem
			{
				IsGem = true,
				GemMaterial = GemMaterial.TourmalineEarth,
				IsFavorite = false,
				PrimaryText = "Tourmaline",
				SecondaryText = "Earth",
				IsReadOnly = true,
				ColorGroup = GemColorType.Brown,
				Description = "Tourmaline is a birthstone for October, along with Opal, and is the gem of the 8th anniversary. Tourmalines have a variety of exciting colors with one of the widest color ranges of any gem, and is known for their hardness and durability."
			},
			new MaterialOptionItem
			{
				IsGem = true,
				GemMaterial = GemMaterial.CZBlack,
				IsFavorite = false,
				PrimaryText = "CZ Black",
				SecondaryText = " ",
				IsReadOnly = true,
				ColorGroup = GemColorType.Black,
				Description = "Jet-black color with lots of sparkle at an affordable price. Lab-created Cubic Zirconia adds drama to a design without a large monetary commitment, freeing you to entertain lots of exciting options."
			},
			new MaterialOptionItem
			{
				IsGem = true,
				GemMaterial = GemMaterial.DiamondBlack,
				IsFavorite = false,
				PrimaryText = "Diamond Black",
				SecondaryText = " ",
				IsReadOnly = true,
				ColorGroup = GemColorType.Black,
				Description = "Diamond is the birthstone for April and the gem that marks the 75th wedding anniversary. When Diamonds are formed with traces of other minerals, rare and beautiful colors can result. These “fancy” colors range from blue and brilliant yellow to red, brown, pale green, pink and violet. Because of their rarity, colored Diamonds are highly desirable and may be quite valuable."
			},
			new MaterialOptionItem
			{
				IsGem = true,
				GemMaterial = GemMaterial.Marcasite,
				IsFavorite = false,
				PrimaryText = "Marcasite",
				SecondaryText = " ",
				IsReadOnly = true,
				ColorGroup = GemColorType.Black,
				Description = "Metallic shine and rich, neutral color make Marcasite the essential accent stone. Gleaming like finely polished silver, Marcasite was highly favored by Art Deco designers of the 20s and 30s. Today, the streamlined geometrics of that period are enjoying renewed popularity, as customers seek the glamour of Hollywood’s “film noir” era and its sophisticated styles."
			},
			new MaterialOptionItem
			{
				IsGem = true,
				GemMaterial = GemMaterial.Onyx,
				IsFavorite = false,
				PrimaryText = "Onyx",
				SecondaryText = " ",
				IsReadOnly = true,
				ColorGroup = GemColorType.Black,
				Description = "Onyx is one of the gems for the 7th anniversary. The velvety black of Onyx makes an unmistakable statement of power, authority and sophistication. A dramatic background for brilliantly colored gems, Onyx can also stand alone for a bold and richly self-assured look, and is a natural for masculine jewelry."
			},
			new MaterialOptionItem
			{
				IsGem = true,
				GemMaterial = GemMaterial.SpinelBlack,
				IsFavorite = false,
				PrimaryText = "Spinel Black",
				SecondaryText = " ",
				IsReadOnly = true,
				ColorGroup = GemColorType.Black,
				Description = "Although rare, Spinels are an affordable gemstone. Not widely known to consumers, Black Spinel is a fantastic choice for those seeking the drama of black-stone jewelry. Without the price tag of Black Diamonds and tougher than Onyx, its naturally-profound color makes the unusual Black Spinel a marvelous choice for bold jewelry designs."
			}
		};
		list.ToList().ForEach(delegate(MaterialOptionItem _)
		{
			_.PrimaryText = TranslationHelper.GetTranslatedGemMaterial(_.PrimaryText.Trim() + " " + _.SecondaryText.Trim());
			_.SecondaryText = string.Empty;
			_.Description = TranslationHelper.GetTranslatedGemMaterial(_.Description);
		});
		if (isListFiltered)
		{
			list = list.Where((MaterialOptionItem _) => !UIHelper.CounterSketchOptions.ExcludedGemMaterialsList.Contains(_.PrimaryText)).ToList();
		}
		return new ObservableCollection<MaterialOptionItem>(list);
	}

	public static DrawingImage GetGemShapeImage(string gemShape)
	{
		DrawingImage result = null;
		ResourceDictionary resourceDictionary = Application.LoadComponent(new Uri("/ResourceDictionaries/GemShapeIcons.xaml", UriKind.Relative)) as ResourceDictionary;
		if (gemShape != null)
		{
			string text;
			switch (gemShape.ToLower())
			{
			case "asscher":
			case "emerald square":
			case "emerald square rotated 45":
				text = "CSGraphicGemChooserSquareEmerald_Default";
				break;
			case "antique cushion":
			case "cushion":
			case "cushion rotated 90":
				text = "CSGraphicGemChooserCushion_Default";
				break;
			case "antique square":
			case "cushion square":
			case "cushion square rotated 45":
				text = "CSGraphicGemChooserSquareCushion_Default";
				break;
			case "emerald":
			case "emerald rotated 90":
				text = "CSGraphicGemChooserEmerald_Default";
				break;
			case "heart":
				text = "CSGraphicGemChooserHeart_Default";
				break;
			case "marquise":
			case "marquise rotated 90":
				text = "CSGraphicGemChooserMarquise_Default";
				break;
			case "oval":
			case "oval rotated 90":
				text = "CSGraphicGemChooserOval_Default";
				break;
			case "pear":
				text = "CSGraphicGemChooserPear_Default";
				break;
			case "round":
				text = "CSGraphicGemChooserRound_Default";
				break;
			case "square":
			case "princess":
			case "princess rotated 45":
				text = "CSGraphicGemChooserPrincess_Default";
				break;
			case "straight baguette":
			case "baguette straight":
				text = "CSGraphicGemChooserBaguette_Default";
				break;
			case "tapered baguette":
			case "baguettetapered":
				text = "CSGraphicGemChooserTaperedBaguette_Default";
				break;
			case "trillion":
			case "trillion curved":
			case "trillion curved 2":
				text = "CSGraphicGemChooserTrillian_Default";
				break;
			case "trillion straight":
			case "trillion straight 2":
				text = "CSGraphicGemChooserTrillianStraight_Default";
				break;
			case "half moon":
				text = "CSGraphicGemChooserHalfMoon_Default";
				break;
			case "calf":
				text = "CSGraphicGemChooserCalf_Default";
				break;
			case "radiant":
				text = "CSGraphicGemChooserRadiant_Default";
				break;
			case "radiant square":
				text = "CSGraphicGemChooserSquareRadiant_Default";
				break;
			case "triangle":
				text = "CSGraphicGemChooserTriangle_Default";
				break;
			case "octagon":
				text = "CSGraphicGemChooserOctagon_Default";
				break;
			default:
				text = "MissingVectorShape";
				break;
			}
			if (resourceDictionary != null && text != "MissingVectorShape" && resourceDictionary.Contains(text) && resourceDictionary[text] is DrawingBrush drawingBrush)
			{
				if (gemShape.Contains(" 45"))
				{
					drawingBrush.Transform = new RotateTransform(45.0, 0.5, 0.5);
				}
				else if (gemShape.Contains(" 90"))
				{
					drawingBrush.Transform = new RotateTransform(90.0, 0.5, 0.5);
				}
				result = new DrawingImage(drawingBrush.Drawing);
			}
		}
		return result;
	}

	public static double GetGemShapeRotationAngle(string gemShape)
	{
		double result = 0.0;
		if (gemShape.Contains(" 45"))
		{
			result = 45.0;
		}
		else if (gemShape.Contains(" 90"))
		{
			result = 90.0;
		}
		return result;
	}

	public static BitmapImage GetImageForGems(Enum value)
	{
		if (_gemMaterialImageCache.ContainsKey((GemMaterial)(object)value))
		{
			return _gemMaterialImageCache[(GemMaterial)(object)value];
		}
		return new BitmapImage();
	}

	public static void UpdateMaterialImageCache()
	{
		foreach (GemMaterial item in (from int enumValue in Enum.GetValues(typeof(GemMaterial))
			where enumValue > 0 && enumValue < 1290
			select enumValue).Cast<GemMaterial>())
		{
			if (!_gemMaterialImageCache.ContainsKey(item))
			{
				_gemMaterialImageCache.Add(item, new BitmapImage(new Uri($"pack://application:,,,/CounterSketch Studio;component/Images/Search/FilterItems/GemColors/{Convert.ToInt32(item)}_{Enum.GetName(item.GetType(), item)}.jpg")));
			}
		}
	}
}
