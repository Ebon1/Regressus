﻿using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria.GameContent;
using Regressus.Projectiles.Minibosses.Vagrant;
using Terraria.DataStructures;
using Regressus.NPCs.Bosses.Vagrant;
using ReLogic.Content;
using Terraria.GameContent.Bestiary;
namespace Regressus.NPCs.Bosses.Vagrant
{
    public class OrbitingHail : ModNPC
    {
        public override string Texture => "Regressus/Projectiles/Minibosses/Vagrant/Hail1";
        public override void SetStaticDefaults()
        {

            NPCID.Sets.TrailCacheLength[NPC.type] = 10;
            NPCID.Sets.TrailingMode[NPC.type] = 0;
        }
        public override void SetDefaults()
        {
            NPC.Size = Vector2.One * 30;
            NPC.scale = 1;
            NPC.aiStyle = -1;
            NPC.lifeMax = 50;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.noTileCollide = true;
            NPC.noGravity = true;
            NPC.knockBackResist = 0f;
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.NightTime,
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Visuals.Moon,
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Visuals.Rain,
                #region "no"
                new FlavorTextBestiaryInfoElement("Hail is a form of solid precipitation.[1] It is distinct from ice pellets (American English \"sleet\"), though the two are often confused.[2] It consists of balls or irregular lumps of ice, each of which is called a hailstone. Ice pellets generally fall in cold weather, while hail growth is greatly inhibited during cold surface temperatures.[3]\r\n\r\nUnlike other forms of water ice precipitation, such as graupel (which is made of rime ice), ice pellets (which are smaller and translucent), and snow (which consists of tiny, delicately-crystalline flakes or needles), hailstones usually measure between 5 mm (0.2 in) and 15 cm (6 in) in diameter.[1] The METAR reporting code for hail 5 mm (0.20 in) or greater is GR, while smaller hailstones and graupel are coded GS.\r\n\r\nHail is possible within most thunderstorms (as it is produced by cumulonimbus),[4] as well as within 2 nmi (3.7 km) of the parent storm. Hail formation requires environments of strong, upward motion of air within the parent thunderstorm (similar to tornadoes) and lowered heights of the freezing level. In the mid-latitudes, hail forms near the interiors of continents, while, in the tropics, it tends to be confined to high elevations.\r\n\r\nThere are methods available to detect hail-producing thunderstorms using weather satellites and weather radar imagery. Hailstones generally fall at higher speeds as they grow in size, though complicating factors such as melting, friction with air, wind, and interaction with rain and other hailstones can slow their descent through Earth's atmosphere. Severe weather warnings are issued for hail when the stones reach a damaging size, as it can cause serious damage to human-made structures, and, most commonly, farmers' crops." +
                "Definition\r\nAny thunderstorm which produces hail that reaches the ground is known as a hailstorm.[5] An ice crystal with a diameter of >5 mm (0.20 in) is considered a hailstone.[4] Hailstones can grow to 15 cm (6 in) and weigh more than 0.5 kg (1.1 lb).[6]\r\n\r\nUnlike ice pellets, hailstones are layered and can be irregular and clumped together.[citation needed] Hail is composed of transparent ice or alternating layers of transparent and translucent ice at least 1 mm (0.039 in) thick, which are deposited upon the hailstone as it travels through the cloud, suspended aloft by air with strong upward motion until its weight overcomes the updraft and falls to the ground. Although the diameter of hail is varied, in the United States, the average observation of damaging hail is between 2.5 cm (0.98 in) and golf ball-sized 4.4 cm (1.75 in).[7]\r\n\r\nStones larger than 2 cm (0.80 in) are usually considered large enough to cause damage. The Meteorological Service of Canada issues severe thunderstorm warnings when hail that size or above is expected.[8] The US National Weather Service has a 2.5 cm (0.98 in) or greater in diameter threshold, effective January 2010, an increase over the previous threshold of .75 in (1.9 cm) hail.[9] Other countries have different thresholds according to local sensitivity to hail; for instance, grape-growing areas could be adversely impacted by smaller hailstones. Hailstones can be very large or very small, depending on how strong the updraft is: weaker hailstorms produce smaller hailstones than stronger hailstorms (such as supercells), as the more powerful updrafts in a stronger storm can keep larger hailstones aloft." +
                "Formation\r\nHail forms in strong thunderstorm clouds, particularly those with intense updrafts, high liquid water content, great vertical extent, large water droplets, and where a good portion of the cloud layer is below freezing 0 °C (32 °F).[4] These types of strong updrafts can also indicate the presence of a tornado.[10] The growth rate of hailstones is impacted by factors such as higher elevation, lower freezing zones, and wind shear.[11]" +
                "Like other precipitation in cumulonimbus clouds, hail begins as water droplets. As the droplets rise and the temperature goes below freezing, they become supercooled water and will freeze on contact with condensation nuclei. A cross-section through a large hailstone shows an onion-like structure. This means the hailstone is made of thick and translucent layers, alternating with layers that are thin, white and opaque. Former theory suggested that hailstones were subjected to multiple descents and ascents, falling into a zone of humidity and refreezing as they were uplifted. This up and down motion was thought to be responsible for the successive layers of the hailstone. New research, based on theory as well as field study, has shown this is not necessarily true.\r\n\r\nThe storm's updraft, with upwardly directed wind speeds as high as 110 mph (180 km/h),[12] blows the forming hailstones up the cloud. As the hailstone ascends it passes into areas of the cloud where the concentration of humidity and supercooled water droplets varies. The hailstone's growth rate changes depending on the variation in humidity and supercooled water droplets that it encounters. The accretion rate of these water droplets is another factor in the hailstone's growth. When the hailstone moves into an area with a high concentration of water droplets, it captures the latter and acquires a translucent layer. Should the hailstone move into an area where mostly water vapor is available, it acquires a layer of opaque white ice.[13]" +
                "Furthermore, the hailstone's speed depends on its position in the cloud's updraft and its mass. This determines the varying thicknesses of the layers of the hailstone. The accretion rate of supercooled water droplets onto the hailstone depends on the relative velocities between these water droplets and the hailstone itself. This means that generally the larger hailstones will form some distance from the stronger updraft where they can pass more time growing.[13] As the hailstone grows it releases latent heat, which keeps its exterior in a liquid phase. Because it undergoes 'wet growth', the outer layer is sticky (i.e. more adhesive), so a single hailstone may grow by collision with other smaller hailstones, forming a larger entity with an" +
                "irregular shape.[15]\r\n\r\nHail can also undergo 'dry growth' in which the latent heat release through freezing is not enough to keep the outer layer in a liquid state. Hail forming in this manner appears opaque due to small air bubbles that become trapped in the stone during rapid freezing. These bubbles coalesce and escape during the 'wet growth' mode, and the hailstone is more clear. The mode of growth for a hailstone can change throughout its development, and this can result in distinct layers in a hailstone's cross-section.[16]\r\n\r\nThe hailstone will keep rising in the thunderstorm until its mass can no longer be supported by the updraft. This may take at least 30 minutes based on the force of the updrafts in the hail-producing thunderstorm, whose top is usually greater than 10 km high. It then falls toward the ground while continuing to grow, based on the same processes, until it leaves the cloud. It will later begin to melt as it passes into air above freezing temperature.[17]\r\n\r\n0:18\r\nHeavy hailstorm at Thakurgaon, Northern Bangladesh (April, 2022)\r\nThus, a unique trajectory in the thunderstorm is sufficient to explain the layer-like structure of the hailstone. The only case in which multiple trajectories can be discussed is in a multicellular thunderstorm, where the hailstone may be ejected from the top of the \"mother\" cell and captured in the updraft of a more intense \"daughter\" cell. This, however, is an exceptional case.[13]" +
                "Factors favoring hail\r\nHail is most common within continental interiors of the mid-latitudes, as hail formation is considerably more likely when the freezing level is below the altitude of 11,000 ft (3,400 m).[18] Movement of dry air into strong thunderstorms over continents can increase the frequency of hail by promoting evaporational cooling which lowers the freezing level of thunderstorm clouds giving hail a larger volume to grow in. Accordingly, hail is less common in the tropics despite a much higher frequency of thunderstorms than in the mid-latitudes because the atmosphere over the tropics tends to be warmer over a much greater altitude. Hail in the tropics occurs mainly at higher elevations.[19]\r\n\r\nHail growth becomes vanishingly small when air temperatures fall below −30 °C (−22 °F) as supercooled water droplets become rare at these temperatures.[18] Around thunderstorms, hail is most likely within the cloud at elevations above 20,000 ft (6,100 m). Between 10,000 ft (3,000 m) and 20,000 ft (6,100 m), 60 percent of hail is still within the thunderstorm, though 40 percent now lies within the clear air under the anvil. Below 10,000 ft (3,000 m), hail is equally distributed in and around a thunderstorm to a distance of 2 nmi (3.7 km).[20]" +
                "Climatology\r\nHail occurs most frequently within continental interiors at mid-latitudes and is less common in the tropics, despite a much higher frequency of thunderstorms than in the mid-latitudes.[21] Hail is also much more common along mountain ranges because mountains force horizontal winds upwards (known as orographic lifting), thereby intensifying the updrafts within thunderstorms and making hail more likely.[22] The higher elevations also result in there being less time available for hail to melt before reaching the ground. One of the more common regions for large hail is across mountainous northern India, which reported one of the highest hail-related death tolls on record in 1888.[23] China also experiences significant hailstorms.[24] Central Europe and southern Australia also experience a lot of hailstorms. Regions where hailstorms frequently occur are southern and western Germany, northern and eastern France, and southern and eastern Benelux. In southeastern Europe, Croatia and Serbia experience frequent occurrences of hail.[25]\r\n\r\nIn North America, hail is most common in the area where Colorado, Nebraska, and Wyoming meet, known as \"Hail Alley\".[26] Hail in this region occurs between the months of March and October during the afternoon and evening hours, with the bulk of the occurrences from May through September. Cheyenne, Wyoming is North America's most hail-prone city with an average of nine to ten hailstorms per season.[27] To the north of this area and also just downwind of the Rocky Mountains is the Hailstorm Alley region of Alberta, which also experiences an increased incidence of significant hail events." +
                "Short-term detection\r\nWeather radar is a very useful tool to detect the presence of hail-producing thunderstorms. However, radar data has to be complemented by a knowledge of current atmospheric conditions which can allow one to determine if the current atmosphere is conducive to hail development.\r\n\r\nModern radar scans many angles around the site. Reflectivity values at multiple angles above ground level in a storm are proportional to the precipitation rate at those levels. Summing reflectivities in the Vertically Integrated Liquid or VIL, gives the liquid water content in the cloud. Research shows that hail development in the upper levels of the storm is related to the evolution of VIL. VIL divided by the vertical extent of the storm, called VIL density, has a relationship with hail size, although this varies with atmospheric conditions and therefore is not highly accurate.[28] Traditionally, hail size and probability can be estimated from radar data by computer using algorithms based on this research. Some algorithms include the height of the freezing level to estimate the melting of the hailstone and what would be left on the ground.\r\n\r\nCertain patterns of reflectivity are important clues for the meteorologist as well. The three body scatter spike is an example. This is the result of energy from the radar hitting hail and being deflected to the ground, where they deflect back to the hail and then to the radar. The energy took more time to go from the hail to the ground and back, as opposed to the energy that went directly from the hail to the radar, and the echo is further away from the radar than the actual location of the hail on the same radial path, forming a cone of weaker reflectivities." +
                "The size of hailstones is best determined by measuring their diameter with a ruler. In the absence of a ruler, hailstone size is often visually estimated by comparing its size to that of known objects, such as coins.[33] Using the objects such as hen's eggs, peas, and marbles for comparing hailstone sizes is imprecise, due to their varied dimensions. The UK organisation, TORRO, also scales for both hailstones and hailstorms.[34]\r\n\r\nWhen observed at an airport, METAR code is used within a surface weather observation which relates to the size of the hailstone. Within METAR code, GR is used to indicate larger hail, of a diameter of at least 0.25 in (6.4 mm). GR is derived from the French word grêle. Smaller-sized hail, as well as snow pellets, use the coding of GS, which is short for the French word grésil.[35]\r\n\r\n\r\nThe largest recorded hailstone in the United States\r\nTerminal velocity of hail, or the speed at which hail is falling when it strikes the ground, varies. It is estimated that a hailstone of 1 cm (0.39 in) in diameter falls at a rate of 9 m/s (20 mph), while stones the size of 8 cm (3.1 in) in diameter fall at a rate of 48 m/s (110 mph). Hailstone velocity is dependent on the size of the stone, its drag coefficient, the motion of wind it is falling through, collisions with raindrops or other hailstones, and melting as the stones fall through a warmer atmosphere. As hailstones are not perfect spheres, it is difficult to accurately calculate their drag coefficient - and, thus, their speed.[36]\r\n\r\nHail records\r\nMegacryometeors, large rocks of ice that are not associated with thunderstorms, are not officially recognized by the World Meteorological Organization as \"hail,\" which are aggregations of ice associated with thunderstorms, and therefore records of extreme characteristics of megacryometeors are not given as hail records.\r\n\r\nHeaviest: 1.02 kg (2.25 lb); Gopalganj District, Bangladesh, 14 April 1986.[37][38]\r\nLargest diameter officially measured: 7.9 in (20 cm) diameter, 18.622 in (47.3 cm) circumference; Vivian, South Dakota, 23 July 2010.[39]\r\nLargest circumference officially measured: 18.74 in (47.6 cm) circumference, 7.0 in (17.8 cm) diameter; Aurora, Nebraska, 22 June 2003.[38][40]\r\nGreatest average hail precipitation: Kericho, Kenya experiences hailstorms, on average, 50 days annually. Kericho is close to the equator and the elevation of 2,200 metres (7,200 ft) contributes to it being a hot spot for hail.[41] Kericho reached the world record for 132 days of hail in one year.[42]" +
                "Hail can cause serious damage, notably to automobiles, aircraft, skylights, glass-roofed structures, livestock, and most commonly, crops.[27] Hail damage to roofs often goes unnoticed until further structural damage is seen, such as leaks or cracks. It is hardest to recognize hail damage on shingled roofs and flat roofs, but all roofs have their own hail damage detection problems.[43] Metal roofs are fairly resistant to hail damage, but may accumulate cosmetic damage in the form of dents and damaged coatings.\r\n\r\nHail is one of the most significant thunderstorm hazards to aircraft.[44] When hailstones exceed 0.5 in (13 mm) in diameter, planes can be seriously damaged within seconds.[45] The hailstones accumulating on the ground can also be hazardous to landing aircraft. Hail is also a common nuisance to drivers of automobiles, severely denting the vehicle and cracking or even shattering windshields and windows unless parked in a garage or covered with a shielding material. Wheat, corn, soybeans, and tobacco are the most sensitive crops to hail damage.[23] Hail is one of Canada's most expensive hazards.[46]\r\n\r\nRarely, massive hailstones have been known to cause concussions or fatal head trauma. Hailstorms have been the cause of costly and deadly events throughout history. One of the earliest known incidents occurred around the 9th century in Roopkund, Uttarakhand, India, where 200 to 600 nomads seem to have died of injuries from hail the size of cricket balls.[47]" +
                "Narrow zones where hail accumulates on the ground in association with thunderstorm activity are known as hail streaks or hail swaths,[48] which can be detectable by satellite after the storms pass by.[49] Hailstorms normally last from a few minutes up to 15 minutes in duration.[27] Accumulating hail storms can blanket the ground with over 2 in (5.1 cm) of hail, cause thousands to lose power, and bring down many trees. Flash flooding and mudslides within areas of steep terrain can be a concern with accumulating hail.[50]\r\n\r\nDepths of up to 18 in (0.46 m) have been reported. A landscape covered in accumulated hail generally resembles one covered in accumulated snow and any significant accumulation of hail has the same restrictive effects as snow accumulation, albeit over a smaller area, on transport and infrastructure.[51] Accumulated hail can also cause flooding by blocking drains, and hail can be carried in the floodwater, turning into a snow-like slush which is deposited at lower elevations.\r\n\r\nOn somewhat rare occasions, a thunderstorm can become stationary or nearly so while prolifically producing hail and significant depths of accumulation do occur; this tends to happen in mountainous areas, such as the July 29, 2010 case[52] of a foot of hail accumulation in Boulder County, Colorado. On June 5, 2015, hail up to four feet deep fell on one city block in Denver, Colorado. The hailstones, described as between the size of bumble bees and ping pong balls, were accompanied by rain and high winds. The hail fell in only the one area, leaving the surrounding area untouched. It fell for one and a half hours between 10 p.m. and 11:30 pm. A meteorologist for the National Weather Service in Boulder said, \"It's a very interesting phenomenon. We saw the storm stall. It produced copious amounts of hail in one small area. It's a meteorological thing.\" Tractors used to clear the area filled more than 30 dump truck loads of hail.[53]" +
                "Research focused on four individual days that accumulated more than 5.9 inches (15 cm) of hail in 30 minutes on the Colorado front range has shown that these events share similar patterns in observed synoptic weather, radar, and lightning characteristics,[54] suggesting the possibility of predicting these events prior to their occurrence. A fundamental problem in continuing research in this area is that, unlike hail diameter, hail depth is not commonly reported. The lack of data leaves researchers and forecasters in the dark when trying to verify operational methods. A cooperative effort between the University of Colorado and the National Weather Service is in progress. The joint project's goal is to enlist the help of the general public to develop a database of hail accumulation depths.[55]" +
                "During the Middle Ages, people in Europe used to ring church bells and fire cannons to try to prevent hail, and the subsequent damage to crops. Updated versions of this approach are available as modern hail cannons. Cloud seeding after World War II was done to eliminate the hail threat,[12] particularly across the Soviet Union, where it was claimed a 70–98% reduction in crop damage from hail storms was achieved by deploying silver iodide in clouds using rockets and artillery shells.[56][57] But these effects have not been replicated in randomized trials conducted in the West.[58] Hail suppression programs have been undertaken by 15 countries between 1965 and 2005.[12][23]")
            });
            #endregion
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPosition, Color drawColor)
        {
            spriteBatch.Reload(BlendState.Additive);
            Texture2D tex = ModContent.Request<Texture2D>("Regressus/Projectiles/Minibosses/Vagrant/Hail1").Value;
            Texture2D glow = ModContent.Request<Texture2D>("Regressus/Projectiles/Minibosses/Vagrant/Hail1_Glow").Value;
            var fadeMult = 1f / NPCID.Sets.TrailCacheLength[NPC.type];
            const float TwoPi = (float)Math.PI * 2f;
            float scale = (float)Math.Sin(Main.GlobalTimeWrappedHourly * TwoPi / 2f) * 0.3f + 0.7f;
            for (int i = 0; i < NPCID.Sets.TrailCacheLength[NPC.type]; i++)
            {
                spriteBatch.Draw(glow, NPC.oldPos[i] - screenPosition + new Vector2(NPC.width / 2f, NPC.height / 2f), new Rectangle(0, 0, glow.Width, glow.Height), Color.LightBlue * (1f - fadeMult * i), NPC.oldRot[i], glow.Size() / 2, scale * (NPCID.Sets.TrailCacheLength[NPC.type] - i) / NPCID.Sets.TrailCacheLength[NPC.type], NPC.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipVertically, 0f);
            }

            spriteBatch.Reload(BlendState.AlphaBlend);
            spriteBatch.Draw(tex, NPC.Center - screenPosition, new Rectangle(0, 0, tex.Width, tex.Height), Color.White, NPC.rotation, tex.Size() / 2, scale, NPC.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipVertically, 0f);
            spriteBatch.Reload(BlendState.Additive);
            for (int i = 0; i < 5; i++)
                spriteBatch.Draw(tex, NPC.Center - screenPosition, new Rectangle(0, 0, tex.Width, tex.Height), Color.White * scale, NPC.rotation, tex.Size() / 2, scale, NPC.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipVertically, 0f);
            spriteBatch.Reload(BlendState.AlphaBlend);
            return false;
        }
        public override Color? GetAlpha(Color drawColor) => Color.White;
        public override void OnKill()
        {
            Projectile.NewProjectile(NPC.GetSource_Death(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<HailExplosion>(), 15, 1);
            Projectile.NewProjectile(NPC.GetSource_Death(), NPC.Center, RegreUtils.FromAToB(NPC.Center, Main.LocalPlayer.Center) * 5f, ModContent.ProjectileType<Hail1>(), 15, 1);
        }
        int a;
        public override void AI()
        {
            NPC center = Main.npc[(int)NPC.ai[0]];
            if (!center.active || center.type != ModContent.NPCType<VoltageVagrant>())
            {
                NPC.active = false;
            }
            NPC.ai[1] += 2f * (float)Math.PI / 600f * 3f;
            NPC.ai[1] %= 2f * (float)Math.PI;
            NPC.Center = center.Center + 100 * new Vector2((float)Math.Cos(NPC.ai[1]), (float)Math.Sin(NPC.ai[1]));
            /*if (NPC.scale < 1)
                NPC.scale += 0.05f;*/
            if (++a % 5 == 0)
            {
                Projectile a = Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<Lightning2>(), 0, 0);
                a.ai[0] = center.whoAmI;
                a.ai[1] = 2;
            }
        }
    }
}
