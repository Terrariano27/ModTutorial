using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ModTutorial.Projectiles;
using Terraria.Enums;
using Microsoft.Xna.Framework;

namespace ModTutorial.Items.Armas
{
	public class LibroTutorial : ModItem
	{
		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Espada Lunar"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
			// Tooltip.SetDefault("una espada lunar. \n" + "Text 2");
		}

		public override void SetDefaults()
		{
            // DefaultToStaff  se encarga de establecer los valores de Item que usan las armas de magia.
            // Coloca el cursor sobre DefaultToStaff en Visual Studio para leer la documentación!
            //
            //El primer numero es la velocidad de lanzado , el segundo numero la velocidad de disparo y el tercer numero es cuanto mana usa
            Item.DefaultToStaff(ModContent.ProjectileType<PRTCristal>(), 7, 20, 11);
            Item.width = 34;
            Item.height = 40;
            Item.UseSound = SoundID.Item71;

            // Un método especial que establece el daño, el retroceso y la probabilidad de golpe crítico adicional.
            //  Esta arma tiene un crítico del 32% que se suma al crítico base del jugador del 4%
            //El primer numero es el daño, el segundo el knokback y el tercer numero el % de critico
            Item.SetWeaponValues(100, 6, 32);

            //Set ShopValues es para poner la rareza del item y su precio (en monedas de cobre)
            Item.SetShopValues(ItemRarityColor.Purple11, 100);
        }

        public override bool? UseItem(Player player)
        {
            // Create 10 dust particles around the player
            for (int i = 0; i < 20; i++)
            {
                // Get a random position within a circle of radius 50 pixels
                float x = player.Center.X + Main.rand.NextFloat(-50f, 50f);
                float y = player.Center.Y + Main.rand.NextFloat(-50f, 50f);

                // Get a random velocity within a range of -3 to 3 pixels per frame
                float vx = Main.rand.NextFloat(-3f, 3f);
                float vy = Main.rand.NextFloat(-3f, 3f);

                // Create a new dust particle with the given position, velocity, size, color, and no gravity
                Dust.NewDust(new Vector2(x, y), 1, 1, ModContent.DustType<Particulas.BrilloCristal>(), vx, vy, 0, default, 1f);
            }

            return true;
        }

        public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.CrystalBlock, 7);
            recipe.AddTile(TileID.LunarCraftingStation);
			recipe.Register();
		}
	}
}