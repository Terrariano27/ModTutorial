//Que librerias usa. De donde se sacará información para este archivo de código.

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ModTutorial.Items.Armas.ArmasV2 //¿Donde esta ubicado tu item en las carpetas del mod? Recuerda actualizarlo si cambias de ubicacion o copias el codigo.
{
	public class PistolaEjemplo : ModItem //Registra tu pistola en el código.
	{
		public override void SetDefaults() {


            Item.DamageType = DamageClass.Ranged; //¿Qué clase de daño es? Melee, Mago, Rango, Invocador.
            Item.damage = 20; //¿Cuánto daño base debe hacer tu arma?
            Item.knockBack = 5f; //¿Qué tanto retroceso debe aplicar tu arma? (Knockback = Retroceso). Entre más alto el valor, más retroceso hará. El proyectil de esta arma hará knockback de la suma de el arma y el proyectil.
            Item.noMelee = true; //Si se establece como verdadero, la arma no actuara como espada y no hará daño al usar la animación.

            Item.width = 62; //El ancho de tu arma (px). Recomendado mantener los valores igual a los de tu textura.	
			Item.height = 32; //El alto de tu arma (px). Recomendado mantener los valores igual a los de tu textura.	
			Item.scale = 0.75f; //Una escala que cambia el tamaño de tu arma al usarse en el juego. 																					
			Item.rare = ItemRarityID.Green; //La rareza de tu arma
			Item.value = 900; //El valor de tu arma en monnedas de cobre.

	
			Item.useTime = 8; //¿Qué tan rápida debe ser tu arma? Entre más alto el numero, más lenta será.
            Item.useAnimation = 8; //¿Cuánto tarda en hacer su animación de ataque tu arma?  Este es independiente a la velocdad de ataque y puede ser diferente valor.
            Item.useStyle = ItemUseStyleID.Shoot; //¿Qué animación usa tu arma? Obtenida de las ya existentes en el juego
            
			Item.autoReuse = true; //¿Deberías poder usar el arma manteniendo apretado el click sin soltarlo? Si esta en 'false', cada vez que quieras atacar deberas dar 1 click.

       
            Item.UseSound = SoundID.MaxMana; //¿Qué sonido debería hacer tu arma al usarse?
           
			Item.shoot = ProjectileID.PurificationPowder; // Esto no afecta al proyectil lanzado, pero se necesita que este para que funcione el disparar.
			Item.shootSpeed = 10f; //¿Con qué velocidad deberia ser lanzado el Proyectil?
            Item.useAmmo = AmmoID.Bullet; // El AmmoID es los conjuntos o el tipo de municion que puede usar el arma. En este caso, puede usar balas.
		}

        public override void AddRecipes() //Añadele una receta (crafteo) a tu arma.
        {
            Recipe recipe = CreateRecipe(); //Crea la receta
            recipe.AddIngredient(ItemID.LunarBar, 25); //Usa 7 barras lunares para craftear.
            recipe.AddTile(TileID.LunarCraftingStation); //En que estación de crafteo se debe hacer. En este caso, la estacion de los pilares/moonlord.
                                                         //Si no quieres que sea necesario una estación de crafteo para craftear esta arma, elimina la linea anterior.

            //recipe.AddTile(TileID.MesaDeCrafteoAqui);
            //recipe.AddIngredient(ItemID.ItemAqui, CantidadAquí);
            //Si quieres añadir más ingredientes o Estaciones de crafteo, puedes usar las 2 lineas anteriores (eliminando los "//".)
            //'CantidadAquí' debe ser un numero (1,2,3...)


            recipe.Register(); //Guarda esta receta para que funcione.
        }

    }
}
//Recuerda que cualquier ID para las lineas de codigo pueden ser obtenidas en la wiki de terraria (IDs de objetos,tiles, animaciones, etc.)
