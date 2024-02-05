using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ModTutorial.Projectiles;

namespace ModTutorial.Items
{
	public class EspadaTutorial : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Espada Lunar"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
			// Tooltip.SetDefault("una espada lunar. \n" + "Text 2");
		}

		public override void SetDefaults()
		{
			Item.damage = 200;
			Item.DamageType = DamageClass.Melee;
			Item.width = 40;
			Item.height = 40;
			Item.useTime = 15; 
			Item.useAnimation = 15;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.knockBack = 6;//20
			Item.value = 100;
			Item.rare = ItemRarityID.Red;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<DarkPRT>();
			Item.shootSpeed = 6f;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.LunarBar, 7);
            recipe.AddTile(TileID.LunarCraftingStation);
			recipe.Register();
		}
	}
}