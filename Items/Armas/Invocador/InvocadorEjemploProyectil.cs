using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace  ModTutorial.Items.Armas.Invocador
{

    public class InvocadorEjemploProyectil : ModProjectile
    {

        //Aqui las cosas se ponen más complicadas, veremos como programar una IA muy básica para el proyectil.
        //Para tener el video listo pronto y no tardar tanto, usaremos la IA que nos da el ExampleMod, hecho por el equipo de tModLoader
        //Usaremos las líneas de codigos encontradas en ExampleMod/Content/Projectiles/Minions/ExampleSimpleMinion.cs 
        // Si quieres buscar este y más ejemplos del tModLoader, estan disponibles en su github.

        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 1;
            //Cuantos frames tiene este invocador (proyectil) en su sprite sheet. En este caso usamos una textura
            //simple, no animada, por lo que la dejaremos en 1.

            ProjectileID.Sets.MinionTargettingFeature[Type] = true;
            // Permite que al hacer click derecho con un arma de invocación , la invocación actual se centre en este. (targets)

            Main.projPet[Type] = true;
            //Especifica que este proyectil no es de arma, si no de mascota, o en este caso, de Invocación.

            ProjectileID.Sets.MinionSacrificable[Type] = true;
            //Permite que el invocador pueda ser intercambiado por otras invocaciones ya summoneadas o viceversa.

            // ProjectileID.Sets.CultistIsResistantTo[Type] = true;
            // Hace que el cultista lunatico sea más resistente a los ataques de este minion, tomando solo 75% de daño de este.
            // Normalmente se usa para este tipo de proyectiles, pero lo saltaremos por ahora.
        }
        public override void SetDefaults()
        {

            Projectile.width = 50; //Manten estos valores iguales al tamaño del la textura de tu proyectil.
            Projectile.height = 50;

            Projectile.tileCollide = false;
            //Le quita la colisión al invocador, así que puede volar por donde quiera. Si es que quieres que tu invoación SI tenga colisión,
            //entnces marcalo como verdadero (true)

            // Todo lo siguiente se usa para cualquier* proyectil de invocación, para asegurarse de que funcione como debe.

            Projectile.friendly = true;
            //Si es verdadero, entonces este proyectil no te hará daño ni a ti ni a otros NPC.
            Projectile.minion = true;
            //Especifica que este proyectil es en realidad una invocación.
            Projectile.DamageType = DamageClass.Summon;
            //Especifica que tipo de daño hace, en este caso, de la clase tipo Invocador.
            Projectile.minionSlots = 1f;
            //Es la cantidad de espacios para invocaciones que usa tu invocación. Normalmente se deja en 1, pero si lo quieres cambiar puedes hacerlo.
            //Cada jugador de terraria empieza con 1 solo slot de invocación. Se pueden aumentar mediante accesorios o pociones.
            //Si lo vas a cambiar, recuerda tambien cambiar el valor que esta en tu item, en la parte de SetStaticDefaults()
            Projectile.penetrate = -1;
            //Siempre necesario para un invocador, debe tener penetración infinita (-1). Si es que no fuera así, tu invocación desapareceria al
            //hacer contacto con cualquier enemigo.

        }

        public override bool? CanCutTiles()
        {
            return false;
            //decide si tu invocacion puede romper vasijas, pasto o cosas así.
        }

        //Para este caso, estamos intentando hacer un minion simple que haga daño por contacto al volar hacia los enemigos
        //Por lo que esta linea de código es obligatoria, para asegurarse de que haga daño al contacto.

        public override bool MinionContactDamage()
        {
            return true;
            //verdadero, para que haga daño por contacto.
        }


        //Aquí las cosas se ponen algo más complicadas. No iré muy a fondo con que hace cada cosa, pero esta "IA" (dividida en partes,
        //ya que el código esta heredado del ExampleMod para apurar la salida del video).
        
        //Esta IA que usaremos hace que la invocación se mantenga sobre el jugador cuando no este atacando, y que persiga a los enemigos
        //cercanos a esta. Cada nueva invocación se pondra un poco más arriba del jugador
        public override void AI()
        {
            Player owner = Main.player[Projectile.owner];

            if (!CheckActive(owner))
            {
                return;
            }
            //si el jugador owner esta activo (vivo), entonces sigue con la IA de proyectil.

            GeneralBehavior(owner, out Vector2 vectorPosicionDescanso, out float distanciaPosicionDescanso);
            SearchForTargets(owner, out bool encontroObjetivo, out float distanciaDelObjetivo, out Vector2 centroDelObjetivo);
            Movement(encontroObjetivo, distanciaDelObjetivo, centroDelObjetivo, distanciaPosicionDescanso, vectorPosicionDescanso);
            Visuals();

            //Para hacer más ordenado el código, los de ExampleMod han decidido dividirlo en partes, las que están mencionadasa aquí.
            //Debes mantener esto si vas a usar este mismo sistema.
        }

        // CheckActive esta para hacer que la invocación desaparezca cuando nos maten o para cuando nos salimos de un server.
        // Además, mantiene la invocacion viva.
        //Recuerda que si vas a copiar el código, debes cambiar el <InvocadorEjemplo---> por tu nombre de la invocación.
        private bool CheckActive(Player owner)
        {
            if (owner.dead || !owner.active)
            {
                //Si el jugador esta muerto o inactivo, quita el buff que creamos.
                owner.ClearBuff(ModContent.BuffType<InvocadorEjemploBuff>());
                //Si no, no hace nada.
                return false;
            }

            if (owner.HasBuff(ModContent.BuffType<InvocadorEjemploBuff>()))
            {
                //Si es que el jugador tiene el buff ,Deja el tiempo del proyectil estático en 2 ticks mientras este el buff activo, haciendo que el proyectil nunca muera
                //Si es que no hay buff, entonces el proyectil desaparece luego de 2 ticks.
                Projectile.timeLeft = 2;
            }

            return true;
        }

        private void GeneralBehavior(Player owner, out Vector2 vectorPosicionDescanso, out float distanciaPosicionDescanso)
        {
            Vector2 posQuieto = owner.Center; //Crea una nueva varible (llamada "posQuieto") tipo Vector2D (x,y) y hace que sea igual al centro del dueño, el jugador en este caso.
            posQuieto.Y -= 12f; // Sube 12 coordenadas o pixeles desde el centro del jugador, lo que equivale a menos de 1 tile (cada tile = 16)
            //Como explique antes, se le resta, ya que en terraria entre mayor la coordenada Y , más abajo vamos.


            //Projectile.minionPos es el número o "ID" de la invocación en el orden que las hacemos aparecer.
            //Crea una variable tipo float (f) llamada offsetYInvocacion
            //Esta parte del código hace que cada nueva invocacion que hagamos summon (si tenemos más de 1) se vayan colocando
            //una arriba de otra.
            //La primera invocación que summoneamos, queda con Projectile.minionPos = 0, luego la segunda con =1, la tercera =2 y así...
            //He diseñado esta fórmula que funciona para la mayoría de invocaciones, ya que usa su valor de altura (Como estamos en el eje Y, usamos altura. Si estuvieramos en eje X usariamos su ancho.)
            // Esta formula funciona así: Suma 12 puntos + la altura de la invocacion dividido en 2 siempre, luego suma una multiplicacion entre el minionPos multiplicado por la altura de la invocación.
            // Así nos aseguramos que "siempre" queden ordenados en fila hacia arriba con la misma distancia entre cada uno sin que se sobrepongan.
            float offsetYInvocacion = 12f + (Projectile.height / 2f) + (Projectile.minionPos * Projectile.height); ;
            posQuieto.Y -= offsetYInvocacion; //Posiciona la invocación sobre el jugador.

            // Según los de ExampleMod, el código a continuación está adaptado del código del "Spazmamini  (ID 388, aiStyle 66)"

            // Teletransporta las invocaciones al jugador si la distancia es muy grande.
            vectorPosicionDescanso = posQuieto - Projectile.Center;
            distanciaPosicionDescanso = vectorPosicionDescanso.Length();


            //Si la distancia es mayor a 1600 píxeles , o 100 tiles/ bloques, entonces teletransporta los invocadores a su owner real.
            if (Main.myPlayer == owner.whoAmI && distanciaPosicionDescanso > 1600f)
            {

                Projectile.position = posQuieto;
                Projectile.velocity *= 0.1f;
                Projectile.netUpdate = true;

                //Ahora, Claude(AI) les va a explicar esto, porque es algo más complicado.
                /*
                 * Projectile.position = posQuieto;: mueve el minion a su posición de reposo. Ojo: position es la esquina superior izquierda del hitbox, mientras que posQuieto se calculó a partir de Center. Por eso queda ligeramente descentrado (unos pocos píxeles), algo que a esa escala no se nota realmente.
                 * Projectile.velocity *= 0.1f;: multiplica la velocidadBase por 0,1, es decir, la reduce al 10%. Es lo mismo que velocity = velocity * 0.1f. Sirve para que no llegue con el impulso que llevaba y salga disparado.
                 * Projectile.netUpdate = true;: avisa al juego de que debe enviar este proyectil a los demás jugadores en el siguiente paquete de red. Sin esto, los otros jugadores seguirían viendo al minion en su sitio antiguo.
                 * 
                 */
            }

            // Si es una invocación que vuela, debes crear estaS variables independiente de cualquier otra cosa, para asegurar que los minion se ajusten.
            //ALERTA: El siguiente código esta generado por Claude AI, puede estar defectuoso, pero por lo que he visto en la práctica, no lo está.
            //  Este código esta preparado para funcionar solo si tus invocaciones se posicionan en una torre arriba del jugador (como se puede observar).
            // Si tus invocaciones se ordenan de otra forma, descarta este código y haz el tuyo o usa el del ExampleMod, considerando que ese usa un orden similar a este, pero en el eje x.
            float velocidadOrden = 0.04f;
            float distanciaMinima = Projectile.height * 0.75f;  // menor que la separación de la formación (height)

            foreach (var other in Main.ActiveProjectiles)
            {
                if (other.whoAmI != Projectile.whoAmI
                    && other.owner == Projectile.owner
                    && other.type == Projectile.type)
                {
                    float difY = Projectile.Center.Y - other.Center.Y;
                    float difX = Projectile.Center.X - other.Center.X;

                    if (Math.Abs(difY) < distanciaMinima && Math.Abs(difX) < Projectile.width)
                    {
                        if (difY > 0f)
                        {
                            // Estoy más abajo que el otro: me empujo hacia abajo
                            Projectile.velocity.Y += velocidadOrden;
                        }
                        else if (difY < 0f)
                        {
                            // Estoy más arriba: me empujo hacia arriba
                            Projectile.velocity.Y -= velocidadOrden;
                        }
                        else
                        {
                            // Exactamente a la misma altura: desempatamos con el índice,
                            // para que cada uno vaya en dirección contraria
                            Projectile.velocity.Y += Projectile.whoAmI < other.whoAmI ? -velocidadOrden : velocidadOrden;
                        }
                    }
                }
            }

            /*
             * NOTA GENERADA POR Claude AI para el foreach que creo, atencion:
            Center en vez de position: position es la esquina superior izquierda, y como todos los minions miden lo mismo da igual, pero Center es más correcto.
            other.type == Projectile.type: el original empujaba cualquier proyectil tuyo (balas, flechas...). Así solo se separan los minions de este tipo. Si quieres que también esquiven a otros minions tuyos, usa other.minion en su lugar.
            Zona difX: solo se empujan si están alineados en horizontal (menos de un width de diferencia), así que dos minions en lados opuestos no interfieren.
            El desempate con whoAmI: cuando dos minions nacen en el mismo punto, difY es 0 exacto. Sin esa rama, ninguno se movería y se quedarían pegados. Con ella, el de menor índice sube y el otro baja.

            Una cosa a vigilar: este empuje y la fuerza que los lleva a su idlePosition compiten entre sí. 
            Si ves que se separan demasiado, baja velocidadOrden o sube un poco distanciaMinima (hasta como máximo Projectile.height). 
            Si los ves temblar, bájalo a 0.01f.
             
             */
        }


        //Esta parte es la que nos importa. Aquí la invocación busca objetivos para atacar.
        private void SearchForTargets(Player owner, out bool encontroObjetivo, out float distanciaDelObjetivo, out Vector2 centroDelObjetivo)
        {
            // Rango de busqueda inicial, es de 600 píxeles en cualquier dirección.
            //especificamos que al inicio no haya ningun target encontrado.
            distanciaDelObjetivo = 600f;
            centroDelObjetivo = Projectile.position;
            encontroObjetivo = false;

            

            // Este código es requerido si quieres que tu invocación funcione con látigos y marcadores (targets)
            if (owner.HasMinionAttackTargetNPC)
            {
                NPC npc = Main.npc[owner.MinionAttackTargetNPC];
                float distanciaentre = Vector2.Distance(npc.Center, Projectile.Center);
                //distanciaentre (valor f): Nos indica la distancia en línea recta (vectorial) entre el centro del NPC target y el centro del invocación.


                if (distanciaentre < 1600f) // ¿está a menos de 1600 px? entonces vale como objetivo
                {
                    distanciaDelObjetivo = distanciaentre; //  guarda esa distancia como la del objetivo actual
                    centroDelObjetivo = npc.Center;
                    encontroObjetivo = true;
                }

                /*Explicado por Claude IA
                  HasMinionAttackTargetNPC es true si el jugador señaló un enemigo a mano (por ejemplo golpeándolo con un látigo de invocador).
                  Main.npc[...] es el array de todos los NPCs, y MinionAttackTargetNPC es el índice (id temporal) del enemigo señalado.
                  Vector2.Distance(a, b) es la distancia entre dos puntos (equivale a (a - b).Length()  ; es el teorema de pitágoras aplicado).
                  Si está a menos de 1600 px, se guarda como objetivo (distanciaDelObjetivo, centroDelObjetivo, encontroObjetivo = true). Tiene prioridad sobre cualquier otro enemigo.
                 */
            }
            //Este código a continuación es el más importante de la IA y el más necesario - es el que busca objetivos.
            if (!encontroObjetivo)
            {
                
                foreach (var npc in Main.ActiveNPCs)
                {
                    // Si no hay objetivo marcado (!encontroObjetivo, recordemos que ! representa un NO), recorre todos los NPCs activos.
                    if (npc.CanBeChasedBy())
                    {
                        // Filtra los NPCs que merece la pena perseguir: enemigos vivos, hostiles, que reciben daño, etc. (descarta NPCs amistosos, muñecos, enemigos invulnerables...).

                        //Creamos 5 variables nuevas.

                        //distanciaentre (valor f): Nos indica la distancia en línea recta (vectorial) entre el centro del NPC target y el centro del invocación
                        //masCercano (true/false): Verifica si la distancia entre el centro del proyectil y el centro del objetivo es mayor a distancia entre. Sirve para saber cual NPC esta más cerca.
                        //enRango (true/false): ¿Este nuevo NPC está más cerca que distanciaDelObjetivo (Distancia max de busqueda)? Al principio ese valor es 600, así que equivale a “¿está a menos de 600 px?”.
                        //  Pero en cuanto se acepta un candidato, distanceFromTarget pasa a ser la distancia a ese candidato, y entonces inRange significa “¿está más cerca que el mejor candidato actual?”. 
                        //enLineaDeVista (true/false): CanHitLine traza una línea entre el minion y el NPC (teniendo en cuenta el tamaño de ambos hitboxes) y devuelve true si no hay bloques sólidos en medio. Es decir, “¿puedo ver a este enemigo?”. (ClaudeAI)
                        //cercanoPorPared (true/false): Excepción para la pared: si el enemigo está a menos de 100 px, vale aunque no haya línea de visión. Como el minion atraviesa bloques, puede cruzar al enemigo por dentro del terreno,
                        //  y sin esta excepción dejaría de verlo y potencialmente soltaría el objetivo a mitad del ataque. (Claude AI)
                        // ^^ No le pongan un valor tan alto a lo que lo compara (no más de 80f recomendaria yo), o si no, su minion atacará a enemigos que están incluso atraves
                        // de grandes trozos de tiles o incluso bajo tierra.
                        
                        float distanciaentre = Vector2.Distance(npc.Center, Projectile.Center);
                        bool masCercano = Vector2.Distance(Projectile.Center, centroDelObjetivo) > distanciaentre;
                        bool enRango = distanciaentre < distanciaDelObjetivo;
                        bool enLineaDeVista = Collision.CanHitLine(Projectile.position, Projectile.width, Projectile.height, npc.position, npc.width, npc.height);
                        bool cercanoPorPared = distanciaentre < 80f;

                        if (((masCercano && enRango) || !encontroObjetivo) && (enLineaDeVista || cercanoPorPared))
                        {

                            //Si más cercano y en rango son verdaderos  O aun no encontro objetivo  
                            //y además está en la línea de vista o Cercano por pared, entonces el nuevo NPC se convierte en la 
                            //que la invocacion le hara target

                            distanciaDelObjetivo = distanciaentre;
                            centroDelObjetivo = npc.Center;
                            encontroObjetivo = true;

                            /* Vista generada por Claude AI!!
                             * Combinación mediante if
                             Se lee así:

                            Parte izquierda: (masCercano && enRango) “es mejor que el candidato actual”, o !encontroObjetivo “todavía no tengo ninguno”.
                            Parte derecha: (enLineaDeVista || cercanoPorPared) “lo veo, o está cercano a mí”.
                            Deben cumplirse ambas partes (&&).

                            Si pasa, el NPC se convierte en el nuevo mejor candidato: se actualizan distanciaDelObjetivo, centroDelObjetivo y encontroObjetivo = true.

                            EJEMPLO

                                Hay tres enemigos con línea de visión, a 500, 300 y 450 px del minion:

                            Enemigo a 500: no hay candidato (!encontroObjetivo), así que entra. Ahora distanciaDelObjetivo = 500.
                            Enemigo a 300: masCercano (500 > 300) y enRango (300 < 500) son true, así que reemplaza al anterior. Ahora distanciaDelObjetivo = 300.
                            Enemigo a 450: masCercano (300 > 450) es false, así que se descarta.

                            Resultado: el minion ataca al de 300 px, el más cercano.
                             */
                        }
                    }
                }
            }

            //Traducido de ExampleMod:
            //«friendly» debe establecerse como «true» para que la invocacion pueda infligir daño por contacto.
            // «friendly» debe establecerse como «false» para que no dañe elementos como los dummy (muneco de pruebas) mientras está inactivo.
            // Ambos aspectos dependen de si tiene un objetivo o no, por lo que aquí solo se realiza una asignación.
            // No necesitas hacer esta asignación si tu invocación ataca a distancia con otros proyectiles en lugar de infligir daño por contacto.
            Projectile.friendly = encontroObjetivo;
        }

        private void Movement(bool encontroObjetivo, float distanciaDelObjetivo, Vector2 centroDelObjetivo, float distanciaPosicionDescanso, Vector2 vectorPosicionDescanso)
        {
            // Definimos dos nuevas variables cruciales del proyectil.
            //VelocidadBase (f): Velocidad que se usará para calcular la velocdad final del proyectil (esta ultima se basa en donde esta el objetivo). Si quieres que ataque más rápido, recuerda tambien cambiar los valores de esta misma variable más abajo.
            //Inercia (f): es lo “pesado” que es girar para el proyectil (más inercia = giros más lentos y suaves).}
            //recomiendo dejar la inercia en valor no tan alto, debido a que suaviza (en mi opinion) demasiado el movimiento, y en este caso
            // no quiero que sea así.
            float velocidadBase = 12f;
            float inercia = 9f;

            if (encontroObjetivo)
            {
                // Si la invocación tiene un target a más de 40 pixeles, entonces ve hacia el aumentando la velocdidad exponencialmente.
                if (distanciaDelObjetivo > 40f)
                {
                    
                    Vector2 direccion = centroDelObjetivo - Projectile.Center;
                    direccion.Normalize();
                    direccion *= velocidadBase;

                    Projectile.velocity = (Projectile.velocity * (inercia - 1) + direccion) / inercia;

                    /* Explicado por Claude AI
                    direccion es el vector del minion al enemigo.
                    Normalize() lo deja con longitud 1 (solo conserva la dirección).
                    *= velocidadBase lo convierte en un vector de longitud del valor de velocidadBase, en este caso: “hacia el enemigo a velocidad 12”.
                    La fórmula de Projectile.velocity es una media ponderada: velocidad actual multiplicado por inercia - 1 (en este caso 8) + velocidad deseada, dividido entre la inercia (en este caso, 8). 
                    Así la velocidad gira poco a poco hacia el enemigo en vez de cambiar de golpe, y eso da la trayectoria curva de “persecución”.
                    La condición > 40f hace que, al estar a menos de 40 px, deje de corregir y siga con su inercia, atravesando al enemigo y dándole la vuelta después. 
                     
                      
                     
                     */
                }
            }
            else
            {
                // Si la invocación no tiene ningún objetivo y está a más de 600 pixeles...
                if (distanciaPosicionDescanso > 600f)
                {
                    // Acelera la invocación para que no tarde tanto en llegar.
                    velocidadBase = 18f;
                    inercia = 60f;
                }
                else
                {
                    // Una vez cerca del jugador, vuelve a su velocidad de descanso.
                    velocidadBase = 4f;
                    inercia = 70f;
                }

                 if (distanciaPosicionDescanso > 40f)
                {
                    //La misma fórmula de antes, pero apuntando a la posición de reposo

                    
                    vectorPosicionDescanso.Normalize();
                    vectorPosicionDescanso *= velocidadBase;
                    Projectile.velocity = (Projectile.velocity * (inercia - 1) + vectorPosicionDescanso) / inercia;
                }
                else //if (Projectile.velocity == Vector2.Zero)  // <-- Heredado de example mod, leer nota de abajo.
                {

                    Projectile.velocity *= 0.92f;


                    // Código heredado de ExampleMod , en mi opinion hace que se muevan demasiado cuando están quietos, así que
                    //lo he cambiado por esto de arriba y lo que queda fuera del if.
                    // Projectile.velocity.X = -0.15f;
                    // Projectile.velocity.Y = -0.05f;
                }

            }

            Projectile.position.X += MathF.Sin(Main.GameUpdateCount * 0.03f) * 0.3f;
        }

        private void Visuals()
        {
            // Gira el sprite según su velocidad horizontal (en radianes), así se inclina un poco hacia donde va.
            Projectile.rotation = Projectile.velocity.X * 0.05f;


            // Código heredado de ExampleMod
            // Lo que hace es que en el caso de que fuera un sprite sheet (sprite animado, para el proyectil) circularia entre los sprites en bucle
            // En resumenn: Una simple animacion en bucle.
            // En este caso lo desactivé porque nuestro sprite para el ejemplo no tiene animación. Pruebalo si es que tiene animacion tu sprite!
            /* int frameSpeed = 5;

            Projectile.frameCounter++;

            if (Projectile.frameCounter >= frameSpeed)
            {
                Projectile.frameCounter = 0;
                Projectile.frame++;

                if (Projectile.frame >= Main.projFrames[Type])
                {
                    Projectile.frame = 0;
                }
            }
            */
            //Explicado por Claude AI
            //Hace que el minion emita luz blanca.
            //ToVector3() convierte el color a valores RGB entre 0 y 1, y * 0.78f baja la intensidad.
            //Puedes quitarla sin problema si no quieres que ilumine.

            //Yo lo dejare, pero cambiare el valor a Purple para que sea una luz morada (coherente con la textura).
            Lighting.AddLight(Projectile.Center, Color.Purple.ToVector3() * 0.78f);
        }





    }




}

//Y aquí hemos terminado. 
/*
 * Nuevamente, Gracias al equipo de tModLoader por estos ejemplos del exampleMod
 * Como pueden ver, he implementado algo de IA en mis explicaciones y ligeramnente en código, para hacerlas más rápido y más entendibles, pero quiero que algo quede
 * claro de una vez: No pienso hacer un video totalmente con IA o con codigo 100% IA, si no que planeo usarla más como una herramienta y enseñarle eso
 * a ustedes mismos. Gracias por segir este tutorial y espero te sirva.
 * Quiero que esta  serie siga siendo de aprender y no de "claude generame este codigo" y listo. En ese caso, para que hacer videos si quiera?
 * Si  necesitas ayuda, puedes acudir a mi discord (discord.gg/NxPu4PQevc) o al de tModLoader (es solo inglés ese.)
 * 
 * Att. Tavox27
 * Este codigo, en su mayoria NO es mio, todos los créditos a ExampleMod. Yo solo modifique algunas cosas para adaptarlo a mi invocacion y estilo.
 * 
 * En el futuro haré un video más en detalle de como funciona el parametro AI () y como hacernos expertos. Nos vemos!
 
 */