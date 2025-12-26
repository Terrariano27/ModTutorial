//Que librerias usa. De donde se sacará información para este archivo de código.
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ModTutorial.Projectiles;

namespace ModTutorial.Items.Armas.ArmasV2 //¿Donde esta ubicado tu item en las carpetas del mod? Recuerda actualizarlo si cambias de ubicacion o copias el codigo.
{
	public class EspadaEjemplo : ModItem //Registra tu espada con su "nombre en el código". Dentro del código, no puede haber nada más con este nombre.
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Tu nombre aquí"); // Por defecto, las letras mayusculas en los nombres dentro del código (linea 8) agregaran espacios al nombre a mostrar (Ej: EspadaLocaWaWo -> Espada Loca Wa Wo. 
			// Puedes poner un nombre a mostrar diferente si gustas, quitando los primeros "//" de la linea 12 y cambiando el nombre como te guste.
			// Tooltip.SetDefault("Tu descripción aquí");
			// Si deseas añadirle una descripción al objeto, puedes hacerlo quitando los primeros "//" de la linea 14 y cambiando la descripción como te guste.
			// Recuerda en estos casos siempre dejar las comillas ""
		}

		public override void SetDefaults()
		{
			Item.DamageType = DamageClass.Melee; //¿Qué clase de daño es? Melee, Mago, Rango, Invocador.
			Item.damage = 200; //¿Cuánto daño base debe hacer tu arma?
			Item.useTime = 15; //¿Qué tan rápida debe ser tu arma? Entre más alto el numero, más lenta será.
			Item.knockBack = 6f; //¿Qué tanto retroceso debe aplicar tu arma? (Knockback = Retroceso). Entre más alto el valor, más retroceso hará.
			Item.autoReuse = true; //¿Deberías poder usar la espada manteniendo apretado el click sin soltarlo? Si esta en 'false', cada vez que quieras atacar deberas dar 1 click. 

			Item.value = 100; //¿Cuántas monedas de cobre base debería venderse tu espada a un  NPC?
			Item.rare = ItemRarityID.Red; //¿Qué rareza debería llevar tu arma? (Legendario, experto, raro,común,basura,  etc.)

			Item.useAnimation = 15; //¿Cuánto tarda en  hacer su animación de ataque tu arma?  Este es independiente a la velocdad de ataque y puede ser diferente valor.
			Item.useStyle = ItemUseStyleID.Swing; //¿Qué animación usa tu arma? Obtenida de las ya existentes en el juego
			Item.UseSound = SoundID.Item1; //¿Qué sonido debería hacer tu arma al usarse?

			Item.height = 40; //El alto de tu arma (px). Recomendado mantener los valores igual a los de tu textura.
			Item.width = 40; //El ancho de tu arma (px). Recomendado mantener los valores igual a los de tu textura.
			Item.scale = 1f; //Una escala que cambia el tamaño de tu arma al usarse en el juego. 	
		}

		public override void AddRecipes() //Añadele una receta (crafteo) a tu arma.
		{
			Recipe recipe = CreateRecipe(); //Crea la receta
			recipe.AddIngredient(ItemID.LunarBar, 7); //Usa 7 barras lunares para craftear.
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

