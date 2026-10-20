using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace ModTutorial.Items.Armas.Invocador
{

    public class InvocadorEjemplo : ModItem
    {
        public override void SetStaticDefaults()
        {
            ItemID.Sets.GamepadWholeScreenUseRange[Type] = true;
            // Permite al jugador apuntar a donde quiera si esta usando un mando tipo xbox / play
            // [Type] es una herramienta dada por tModLoader que evita que esta propiedad se aplique a todos los items
            // y a cambio la aplica solo al actual.
            ItemID.Sets.LockOnIgnoresCollision[Type] = true;
            //Permite que el jugador la use a pesar de que haya una pared o colisión de por medio.

            ItemID.Sets.StaffMinionSlotsRequired[Type] = 1f;
            //Cuantos slots requiere para invocarse este minion. Normalmente se deja en 1, pero si lo vas a cambiar, tambien recuerda
            //cambiar el valor en el SetDefaults() del Proyectil!! Debe ser el mismo valor en ambos.

        }

        public override void SetDefaults()
        { 
        
            Item.damage = 165;
            Item.knockBack = 1f;
            Item.mana = 8;

            Item.width = 32;
            Item.height = 32;

            Item.useTime = 22;
            Item.useAnimation = 22;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.UseSound = SoundID.Item68;

            Item.value = Item.sellPrice(silver: 50);
            Item.rare = ItemRarityID.Expert;

            // 100% necesarios para tu arma invocador
            Item.noMelee = true;
            //Evita el daño melee
            Item.DamageType = DamageClass.Summon;
            // Aplica la clase summon para que los modificadores de daño se apliquen correctamente.
            Item.buffType = ModContent.BuffType<InvocadorEjemploBuff>();
            //Aplica el buff de la invocación cuando se use, sin tiempo.
            Item.shoot = ModContent.ProjectileType<InvocadorEjemploProyectil>();
            // Para los proyectiles siempre se usa shoot (disparar) , auqnue no estemos "disparando" realmente.
            // Invoca nuestra Proyectil (el cual es nuestro invocación).

        }
        //ModifyShootStats es para cambiar como funciona el lanzado de proyectiles.
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            //Aquí puedes cambiar donde quieres que aparezca tu invocacion
            //Normalmente se usa la posición del cursor, de la siguiente manera:
            /*
             * 
            position = Main.MouseWorld;
            player.LimitPointToPlayerReachableArea(ref position);
            
            
             */
            //Pero para este tutorial lo haremos aparecer siempre bajo el jugador. Si no te gusta esto, usa las lineas de código
            // que he dejado aquí arriba, sacandolas del área de notación.
            //player.LimitPointToPlayerReach... limita el spawneo del proyectil a donde máximo rango tenga el jugador con el mouse.

            //Como hacerlo spawnear siempre debajo tuyo:

            position = player.Center + new Vector2(0f, 48f);

            //esta linea hará que el invocador siempre aparezca a los pies del jugador, 48 pixeles por debajo del centro del jugador.
            //Si deseas, puedes cambiar los valores del Vector2 (Vector 2d, x - y) para hacerlo spawnear en la posición que quieras 
            //referente al jugador, como por ejemplo 48 pixeles sobre el centro del jugador, 50 pixeles hacia la derecha  seria:
            // position = player.Center + new Vector2(50f, -48f)

            //DATO ÚTIL DE AQUÍ EN ADELANTE: En terraria los valores "X" crecen hacia la derecha y disminuyen hacia la izquierda.
            //Los valores "Y" crecen hacia abajo y disminuyen hacia arriba. 


        }
        //Shoot es para cambiar parametros del lanzamiento de proyectiles.
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            player.AddBuff(Item.buffType, 2);
            return true;

            //Esto es necesario para que el buff de nuestro invocador se aplique correctamente al jugador, sin excepciones.
            //El numero 2 es la duración en ticks de nuestro buff, en este caso 2, que es la minima (nuestro buff sera infinito como dije antes)
            //El Item.buffType hace referencia al buff que mencionamos antes en SetDefaults ()
            //return true es necesario para que siempre se aplique.
        }

        //Finalmente, creamos nuestra receta. En este caso yo usaré luminita

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.LunarBar, 5);
            recipe.AddIngredient(ItemID.FragmentStardust, 12);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }


    }

}