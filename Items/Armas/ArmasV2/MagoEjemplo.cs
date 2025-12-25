//Que librerias usa. De donde se sacará información para este archivo de código.
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ModTutorial.Items.Armas.ArmasV2 //¿Donde esta ubicado tu item en las carpetas del mod? Recuerda actualizarlo si cambias de ubicacion o copias el codigo.
{
	public class MagoEjemplo : ModItem //Registra tu arma en el código.
	{
		public override void SetStaticDefaults()
		{
            // DisplayName.SetDefault("Tu nombre aquí"); // Por defecto, las letras mayusculas en los nombres dentro del código (linea 8) agregaran espacios al nombre a mostrar (Ej: EspadaLocaWaWo -> Espada Loca Wa Wo. 
            // Puedes poner un nombre a mostrar diferente si gustas, quitando los primeros "//" de la linea 12 y cambiando el nombre como te guste.
            // Tooltip.SetDefault("Tu descripción aquí");
            // Si deseas añadirle una descripción al objeto, puedes hacerlo quitando los primeros "//" de la linea 14 y cambiando la descripción como te guste.
            // Recuerda en estos casos siempre dejar las comillas ""
        }
/* NOTA IMPORTANTE!
 * Poner en este codigo un proyectil vanilla para no irse mucho dentro del codigo custom.
 */
        public override void SetDefaults()
		{

            Item.damage = 30;  //¿Cuánto daño base debe hacer tu arma?
            Item.knockBack = 7; //¿Qué tanto retroceso debe aplicar tu arma? (Knockback = Retroceso). Entre más alto el valor, más retroceso hará.
            Item.shoot = ProjectileID.WoodenBoomerang; //¿Qué Proyectil debería lanzar el item al usarse? 
            Item.shootSpeed = 5; //¿Con qué velocidad deberia ser lanzado el Proyectil?
            Item.DamageType = DamageClass.Magic; //¿Qué clase de daño es? Melee, Mago, Rango, Invocador.
            Item.useTime = 15; //¿Qué tan rápida debe ser tu arma? Entre más alto el numero, más lenta será.
            Item.autoReuse = true; //¿Deberías poder usar el arma manteniendo apretado el click sin soltarlo? Si esta en 'false', cada vez que quieras atacar deberas dar 1 click. 
            Item.noMelee = true; //Si se establece como verdadero, la arma no actuara como espada y no hará daño al usar la animación.
            Item.mana = 10; //¿Cuanta maná base gasta tu item al usarse?

            Item.rare = ItemRarityID.LightRed; //¿Qué rareza debería llevar tu arma? (Legendario, experto, raro,común,basura,  etc.)
            Item.value = 500; //¿Cuántas monedas de cobre base debería venderse tu espada a un  NPC? 

            Item.UseSound = SoundID.Item43; //¿Qué sonido debería hacer tu arma al usarse?
            Item.useStyle = ItemUseStyleID.Shoot; //¿Qué animación usa tu arma? Obtenida de las ya existentes en el juego
            Item.useAnimation = 15; //¿Cuánto tarda en hacer su animación de ataque tu arma?  Este es independiente a la velocdad de ataque y puede ser diferente valor.

            Item.width = 34; //El ancho de tu arma (px). Recomendado mantener los valores igual a los de tu textura.
            Item.height = 40; //El alto de tu arma (px). Recomendado mantener los valores igual a los de tu textura.


        }


        public override void AddRecipes() //Añadele una receta (crafteo) a tu arma.
        {
			Recipe recipe = CreateRecipe(); //Crea la receta
            recipe.AddIngredient(ItemID.WoodenBoomerang, 2);//Usa 2 boomerang de madera para craftear
            recipe.AddIngredient(ItemID.Book, 1); // Usa 1 libro para craftear
            recipe.AddTile(TileID.LunarCraftingStation); //En que estación de crafteo se debe hacer. En este caso, la estacion de los pilares/moonlord.
                                                         //Si no quieres que sea necesario una estación de crafteo para craftear esta arma, elimina la linea anterior.
            recipe.Register(); //Guarda esta receta para que funcione.
        }
	}
}
//Recuerda que cualquier ID para las lineas de codigo pueden ser obtenidas en la wiki de terraria (IDs de objetos,tiles, animaciones, etc.)