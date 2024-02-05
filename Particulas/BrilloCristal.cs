using Terraria;
using Terraria.ModLoader;

namespace ModTutorial.Particulas
{
	public class BrilloCristal : ModDust
	{
		public override void OnSpawn(Dust dust) {
			dust.velocity *= 0.4f; // Realentizar la velocidad de la particula , multiplicando x0.4 la velocidad inicial
			dust.noGravity = true; // Hace que la particula no tenga gravedad
			dust.noLight = false; // decidir si la particula emite luz (True = no emite luz, false = si emite luz)
			dust.scale *= 3.5f; //multiplicar el tamaño inicial de la particula  x1.5
		}

		public override bool Update(Dust dust) { // Se usa cada vez que la particula se actualiza, cosas para que se vea más "bonita"
			dust.position += dust.velocity;
			dust.rotation += dust.velocity.X * 0.15f;
			dust.scale *= 0.99f;

			float light = 0.35f * dust.scale;

			Lighting.AddLight(dust.position, light, light, light);

			if (dust.scale < 0.5f) {
				dust.active = false;
			}

			return false; //regresar falso para evitar el comportamiento vanilla
		}
	}
}
