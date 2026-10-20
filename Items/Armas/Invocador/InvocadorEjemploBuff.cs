using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace ModTutorial.Items.Armas.Invocador
{

    public class InvocadorEjemploBuff : ModBuff
    {

        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = true;
            //Esto evita que el buff se guarde al salir del mundo. Crucial si es que es una invocación.
            Main.buffNoTimeDisplay[Type] = true;
            //Con esta linea, el tiempo de duración del buff se ocultará.
        }

        //Update es lo que verifica o hace el buff en cada tick del juego.
        public override void Update(Player player, ref int buffIndex)
        {


            if (player.ownedProjectileCounts[ModContent.ProjectileType<InvocadorEjemploProyectil>()] > 0)
            {
                player.buffTime[buffIndex] = 18000;
            }
            else
            {
                player.DelBuff(buffIndex);
                buffIndex--;

            }

            //Primero verificamos cuantos proyectiles del invocador que creamos (ModContent.ProjectileType<TuProyectilAqui>) hay.
            //Si es que el número resulta ser > mayor que 0, entonces manteneos el buff (un tiempo altisimo, que cada tick se reinicia
            //para que sea imposible que se quite el buff.
            //else (de otra forma) hace que si es que no hay ningun proyectil summoneado, entonces elimina el buff actual del jugador.


        }
        //Eso es todo lo que necesita nuestro buff. Simple.
    }
}
