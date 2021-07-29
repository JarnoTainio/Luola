using System.Collections.Generic;

public class Translation
{
  public static Dictionary<string, string[]> dictionary = new Dictionary<string, string[]> {

    {"thankYou", new string[]{"Thank you!", "Kiitos!" } },
    {"newGame", new string[]{"New game", "Uusi peli" } },
    {"return", new string[]{"Back", "Takaisin" } },
    {"play", new string[]{"Play", "Pelaa" } },
    {"continue", new string[]{"Continue", "Jatka" } },
    {"settings", new string[]{"Settings", "Asetukset" } },
    {"profile", new string[]{"Profile %index%", "Peli %index%" } },
    {"profileMenu", new string[]{"Profiles", "Profiilit" } },
    {"masterVolume", new string[]{"Master", "Äänitaso" } },
    {"musicVolume", new string[]{"Music", "Musiikki" } },
    {"sfxVolume", new string[]{"sfx", "Efektit" } },
    {"cancel", new string[]{"Cancel", "Peruuta" } },
    {"delete", new string[]{"Delete", "Poista" } },
    {"confirmDeleteProfile", new string[]{"Are you sure you want to delete this profile?", "Haluatko varmasti poistaa tämän profiilin?" } },
    {"victory", new string[]{"Victory!", "Voitto!" } },
    {"gameOver", new string[]{"Game over", "Peli päättyi" } },
    {"rooms", new string[]{"rooms", "huoneita" } },
    {"bossKills", new string[]{"boss kills", "pomot" } },
    {"overkill", new string[]{"overkill", "ylimääräinen vahinko" } },
    {"powerLevel", new string[]{"level", "kokemustaso" } },
    {"power", new string[]{"experience", "kokemus" } },
    {"total", new string[]{"total", "yhteensä" } },
    {"inventory", new string[]{"inventory", "reppu" } },
    {"fight", new string[]{"fight!", "taistele!" } },
    {"movesRemaining", new string[]{"%moves% moves remaining!", "%moves% siirta jäljellä" } },
    {"playCombat", new string[]{"Play! (%moves%)", "Pelaa! (%moves%)" } },
    {"level", new string[]{"level", "taso" } },
    {"chooseReward", new string[]{"choose treasure", "Valitse aarre" } },
    {"chooseTwoRewards", new string[]{"choose two treasures", "Valitse kaksi aarretta" } },
    {"select", new string[]{"select", "valitse" } },
    {"potential", new string[]{"potential", "itsevarmuus" } },
    {"attack", new string[]{"attack", "hyökkäys" } },
    {"defence", new string[]{"defence", "puolustus" } },
    {"gold", new string[]{"gold", "kultaa" } },
    {"energy", new string[]{"energy", "energia" } },
    {"life", new string[]{"life", "elinvoimaa" } },
    {"credits", new string[]{"credits", "credits" } },
    {"unlocked", new string[]{"Unlocked!", "Avattu!" } },
    {"currentFloor", new string[]{"Floor %floor%", "Kerros %floor%" } },
    {"selectHero", new string[]{"Select your hero", "Valitse sankarisi" } },
    {"toMenu", new string[]{"Return to menu", "Palaa päävalikkoon" } },
    {"abandonRun", new string[]{"Abandon run", "Hylkää peli" } },
    {"abandonRunConfirm", new string[]{"Abandon run?", "Hylkää peli?" } },
    {"abandon", new string[]{"Abandon", "Hylkää" } },
    {"menu", new string[]{"Menu", "Valikko" } },
    {"startGame", new string[]{"Start game", "Aloita peli" } },
    {"watchAdd", new string[]{"Watch Ad", "Katso mainos" } },
    {"watchAddSupport", new string[]{"Support developer\nWatch Ad", "Tue ohjelmoijaa\nKatso mainos" } },
    {"heroesUnlocked", new string[]{"%count% / %total% unlocked", "%count% / %total% avattu" } },
    {"healAll", new string[]{"Heal to full life", "Parane kaikki vahinko" } },
    {"gem", new string[]{"Gem", "Jalokivi" } },
    {"item", new string[]{"Item", "Esine" } },
    {"ability", new string[]{"Ability", "Toiminta" } },
    {"restLegend", new string[]{"Resting place", "Lepopaikka" } },
    {"legend", new string[]{"Legend", "Legenda" } },
    {"eventLegend", new string[]{"Event", "Tapahtuma" } },
    {"bossLegend", new string[]{"Boss fight", "Pomotaistelu" } },
    {"eliteLegend", new string[]{"Elite monster: + gold and experience tokens.", "Eliitti hirviö: +2 kultaa ja kokemusta." } },
    {"fightLegend", new string[]{"Fight", "Taistelu" } },
    {"goldLegend", new string[]{"Fight: More gold and higher monster life. ", "Taistelu: Enemmän kultaa ja vihollisella enemmän sydämiä." } },
    {"powerLegend", new string[]{"Fight: More experience and higher monster life. ", "Taistelu: Enemmän kokemusta ja vihollisella enemmän sydämiä." } },
    {"damageTaken", new string[]{"Damage taken", "Otettu vahinko" } },
    {"timePlayed", new string[]{"Run time", "Peliaika" } },
    {"heroLevel", new string[]{"Hero level", "Sankaritaso" } },
    {"maximumLifeModifiers", new string[]{"Maximum life modifiers", "Maksimielinvoima kerroin" } },
    {"roomGold", new string[]{"Room gold", "Kultaa huoneissa" } },
    {"roomPower", new string[]{"Room experience", "Kokemusta huoneissa" } },
    {"powerRequiredModifier", new string[]{"Experience required for level", "Vaaditut kokemuspisteet tasoon" } },
    {"rewardsBought", new string[]{"Rewards bought", "Aarteita ostettu" } },
    {"attackDecay", new string[]{"Attack decay", "Hyökkäyksen vähentyminen" } },
    {"defenceDecay", new string[]{"Defence decay", "Puolustuksen vähentyminen" } },
    {"redGemsModifier", new string[]{"Red gems", "Punaiset jalokivet" } },
    {"blueGemsModifier", new string[]{"Blue gems", "Siniset jalokivet" } },
    {"yellowGemsNoTokens", new string[]{"Yellow gems (no gold left)", "Keltaiset jalokivet (ei kultaa)" } },
    {"grayGemsNoTokens", new string[]{"White gems (no experience left)", "Valkoiset jalokivet (ei kokemusta)" } },
    {"chestPriceModifier", new string[]{"Treasure cost", "Aarteiden hinta" } },
    {"monsterLife", new string[]{"Monster life", "Hirviöiden elinvoima" } },
    {"monsterAgro", new string[]{"Monster aggression", "Hirviöiden raivostuminen" } },
    {"bloodMagic", new string[]{"Blood magic", "Verimagia" } },
    {"energyShield", new string[]{"Energy shield", "Energiasuoja" } },
    {"lifeGrowth", new string[]{"Life growth (+2 max life / level)", "Kasvuvoima (+2 maksimielinvoimaa/taso)" } },
    {"action", new string[]{"action", "siirto" } },
    {"escape", new string[]{"escape", "pakene" } },
    {"profile_level", new string[]{"Level", "Taso" } },
    {"ads_watched", new string[]{"Ads watched", "Mainoksia katsottu" } },
    {"experience", new string[]{"Experience", "Kokemus" } },
    {"play_time_total", new string[]{"Total play time", "Peliaika yhteensä" } },
    {"runs", new string[]{"Runs", "Pelikertoja" } },
    {"victories", new string[]{"Victories", "Voitot" } },
    {"coming_soon", new string[]{"Coming soon", "Tulossa pian" } },

    {"infoTitle", new string[]{"Info", "Tiedot" } },
    {"difficultyTitle", new string[]{"Difficulties", "Vaikeustasot" } },
    {"difficultyText", new string[]{
      "<u>Easy</u>\n-20% monster life and monsters enrage one turn slower.\n\n<u>Medium</u>\nNo difficulty modifiers\n\n<u>Hard</u>\n+25% monster life and monsters enrage one turn faster.",
      "<u>Helppo</u>\n-20% vihollisten elinvoima ja viholliset suuttuvat yhden vuoron hitaammine.\n\n<u>Normaali</u>\nEi muutoksia vaikeuteen\n\n<u>Vaikea</u>\n+25% vihollisten elinvoima ja viholliset suuttuvat yhden vuoron nopeammin." }
    },
    {"difficulty", new string[]{"Difficulty", "Vaikeustaso" } },
    {"difficulty0", new string[]{"Easy", "Helppo" } },
    {"difficulty1", new string[]{"Medium", "Normaali" } },
    {"difficulty2", new string[]{"Hard", "Vaikea" } },

    {"inCombat", new string[]{"Floor %floor% Room %room%\nIn combat", "Kerros %floor% Huone %room%\nTaistelussa" } },
    {"inReward", new string[]{"Floor %floor% Room %room%\nSelecting reward", "Kerros %floor% Huone %room%\nPalkintoa valitsemassa" } },
    {"inMap", new string[]{"Floor %floor% Room %room%\nIn map", "Kerros %floor% Huone %room%\nKartalla" } },
    {"inEvent", new string[]{"Floor %floor% Room %room%\nIn event", "Kerros %floor% Huone %room%\nTapahtumassa" } },
    {"inNewGame", new string[]{"Selecting hero", "Sankaria valitsemassa" } },
    {"inUnknown", new string[]{"???", "???" } },

    {"tabBasics", new string[]{"Basics", "Perusteet" } },
    {"tabResources", new string[]{"Resources", "Värit" } },
    {"tabAbilities", new string[]{"Abilities", "Kyvyt" } },

    // COMBAT TUTORIAL
    {"tutorialCombatTitle", new string[]{"How to play", "Peliohjeet" } },
    {"tutorialCombatBasics", new string[]{"Match three or more gems in line to gain resources.", "Yhdistä kolme tai useampi samanväristä jalokiveä saadaksesi resursseja." } },
    {"tutorialCombatRound", new string[]{"<u>Round</u>\n-Perform three gem moves\n-Hero attacks\n-Monster attacks\n-new round starts", "<u>Kierros</u>\n-Tee kolme siirtoa\n-Sankari hyökkää\n-Hirviö hyökkää\n-uusi kierros alkaa" } },
    {"tutorialCombatTokens", new string[]{
      "<u>Tokens</u>\nEach combat has limited amount of gold and experience available. These tokens are shown on left and right side of the gem board. Try to collect them all before defeating the monster.", 
      "<u>Kerättävät</u>\nJokaisessa taisteluissa on rajallinen määrä kultaa ja kokemusta saatavilla. Näet jäljellä olevat kerättävät timanttien vasemmalla ja oikealla puolella. Koita kerätä ne kaikki, ennen kuin voitat hirviön." 
    } },
    {"tutorialCombatAbility", new string[]{
      "Below hero's resources are ability buttons. These abilities can be used by clicking on them and having enough energy to pay for it. You can find more info on your abilities by clicking bagback button on top-right of the combat screen.", 
      "Sankarin sydämien, hyökkäyksen ja puolustuksen alapuolella on hänen kykynsä. Jos sinulla on tarpeeksi energiaa, niin voit aktivoida kyvyn painamalla sitä. Löydät tarkemmat tiedot kyvyistäsi painamalla reppu -painiketta taistelunäkymä oikeasta ylälaidasta." 
    } },
    {"tutorialCombatAbilityCharges", new string[]{
      "<u>Charges</u>\nSome abilities can be used as long as you have enough energy, but others have limited number of uses. These uses are shown by small squares on ability buttons. Green charges refresh after each round and blue charges refresh only at the start of each encounter.", 
      "<u>Lataukset</u>\nJoitakin kykyjä voit käyttää niin kauan kun sinulla on siihen tarvittava energia, mutta toisissa kyvyissä on rajallinen määrä käyttöjä. Käyttöjen määrän näkee kyvyn alareunassa olevien latauskuvakkeiden määrästä. Vihreät lataukset palautuvat jokaisen kierron alussa ja siniset ainoastaan hirviöhuoneen alussa. " 
    } },

    {"tutorialLife", new string[]{"<color=red>Life</color>: Game ends if your life is reduced to zero.", "<color=red>Elinvoima</color>: Häviät pelin, jos elinvoimasi putoaa nollaan." } },
    {"tutorialAttack", new string[]{"<color=red>Attack</color>: Causes damage to enemy at the end of round.", "<color=red>Hyökkäys</color>: Aiheuttaa viholliseen vahinkoa kierroksen päätteeksi." } },
    {"tutorialDefence", new string[]{"<color=blue>Defence</color>: Reduces incoming damage.", "<color=blue>Puolustus</color>: Vähentää tulevaa vahinkoa." } },
    {"tutorialEnergy", new string[]{"<color=green>Energy</color>: Use to activate abilities.", "<color=green>Energia</color>: Käytä aktivoidaksesi kykyjäsi." } },
    {"tutorialPotential", new string[]{"<color=#57004B>Potential</color>: Doubles orbs gained from next gem matching.", "<color=#57004B>Itsevarmuus</color>: Tuplaa saadut resurssit seuraavasta timanttien yhdistämisestä." } },
    {"tutorialGold", new string[]{"<color=yellow>Gold</color>: Can be used to buy extra reward after combat.", "<color=yellow>Kulta</color>: Käytetään aarteiden ostamiseen taistelun jälkeen." } },
    {"tutorialPower", new string[]{"<color=#ff00ff>Experience</color>: Collect to gain levels. At the start of every combat you gain energy equal to your level.", "<color=#ff00ff>Kokemus</color>: Kerää saadaksesi tasoja. Taistelun alussa saat energiaa kokemustasosi verran." } },
    {"tutorialAggro", new string[]{"<color=black>Rage</color>: Enemy gains bonus to it's actions based on it's rage.", "<color=black>Raivo</color>: Vihollisen toiminnot vahvistuvat sen raivon mukaisesti." } },

    // REWARD TUTORIAL
    {"tutorialRewardChest", new string[]{"Pay shown cost to take two rewards instead of one. Each bought chest makes next one cost more.", "Maksa arkun osoittama hinta valitaksesi kaksi aarretta yhden sijaan. Hinta kasvaa jokaisen ostetun arkun jälkeen." } },
    {"tutorialRewardReroll", new string[]{"Pay one gold to reroll offered rewards.", "Maksa yksi kulta arpoaksesi tarjolla olevat aarteet uudestaan." } },
    {"tutorialRewardTitle", new string[]{"Reward info", "Palkkio-ohjeet" } },
    {"tutorialReward", new string[]{"Choose one of the offered rewards. Choose wisely, because each item and ability reward will be offered only once.", "Valitse yksi tarjotuista aarteista. Valitse harkiten, sillä jokainen esine ja kyky -aarre tarjotaan vain yhden kerran." } },


    // HEROES
    {"heroUnlocked", new string[]{"Hero unlocked!", "Sankari avattu!" } },
    {"heroDefaultDescription", new string[]{"Default Luola experience.", "Luolan oletussankari." } },

    {"heroUnlockWin", new string[]{"Defeate last boss", "Voita viimeinen pomo" } },
    {"heroUnlockWinDescription", new string[]{"Larger, richer and more dangerous dungeon.", "Suurempi, rikkaampi ja vaarallisempi luolasto." } },

    {"heroUnlockGold", new string[]{"Have 100 gold", "Omista 100 kultaa" } },
    {"heroUnlockGoldDescription", new string[]{"Use gold to grow stronger.", "Käytä kultaa kasvaaksesi vahvemmaksi." } },

    {"heroUnlockPower", new string[]{"Reach hero level 15", "Saavuta sankaritaso 15" } },
    {"heroUnlockPowerDescription", new string[]{"Use energy shield and potential orbs to survive.", "Selviydy luolasta energiasuojasi ja itsevarmuutesi avulla." } },

    {"heroUnlockLife", new string[]{"Have maximum life of 40", "Saavuta maksimielinvoima 40" } },
    {"heroUnlockLifeDescription", new string[]{"Life can be used as energy and your maximum life increases when you gain a level.", "Voit käyttää elinvoimaasi energiana ja maksimielinvoimasi kasvaa tasojesi myötä." } },

    {"heroUnlockDamage", new string[]{"Have attack 20", "Saavuta hyökkäysarvo 20" } },
    {"heroUnlockDamageDescription", new string[]{"Life is a resource to use and revenge is a skill you have mastered.", "Elinvoima on vain resurssi ja olet kostamisen mestari." } },

    {"heroUnlockNoDamage", new string[]{"Win the game without taking any damage.", "Voita peli ottamatta yhtään vahinkoa." } },
    {"heroUnlockNoDamageDescription", new string[]{"So much gold and experience and huge dungeon, but only one life.", "Niin paljon kultaa ja kokemusta, sekä erittäin suuri luolasto, mutta vain yksi elämä." } },

    {"heroUnlockOverkill", new string[]{ "Cause 1000 overkill damage\n(%overkill% / 1000)", "Aiheuta 1000 pistettä ylimääräistä vahinkoa\n(%overkill% / 1000)" } },
    {"heroUnlockOverkillDescription", new string[]{"Not implemented", "Ei toteutettu" } },

    {"heroUnlockSingleAdd", new string[]{"Watch advertisement", "Katso mainos" } },
    {"heroUnlockAdd", new string[]{"Watch %required% ads.\n(%adds% / %required%)", "Katso %required% mainosta.\n(%adds% / %required%)" } },
    {"heroUnlockAdd0Description", new string[]{"Eat or be eaten.", "Syö tai tule syödyksi." } },
    {"heroUnlockAdd1Description", new string[]{"Slow and tough as snail. Can control gems for it's advantage.", "Hidas ja kova kuin etana. Hallitsee kaikki jalokivien siirtotemput." } },
    {"heroUnlockAdd2Description", new string[]{"Your stinger keeps any damage you caused to your enemies, but they also have triple health.", "Pidät kaiken hyökkäyksen minkä aiheutit vastustajaasi, mutta vihollisilla on kolminkertaiset elinvoimat." } },
    {"heroUnlockAdd3Description", new string[]{"Coming soon", "Tulossa pian" } },
    {"heroUnlockAdd4Description", new string[]{"Much larger gem board, but you must match 4.", "Paljon suurempi jalokivi pelilauta, mutta sinun täytyy yhdistää neljä jalokiveä." } },
    {"heroUnlockAdd5Description", new string[]{"Coming soon", "Tulossa pian" } },
    {"heroUnlockAdd6Description", new string[]{"Coming soon", "Tulossa pian" } },
    {"heroUnlockAdd7Description", new string[]{"Coming soon", "Tulossa pian" } },

    {"creditsText", new string[]{
      "<b>Made by:</b>\nJarno Tainio\n\n<b>Playtesting:</b>\nMarianne, Aaron, Iiris and Aleksi",
      "<b>Tehnyt:</b>\nJarno Tainio\n\n<b>Pelitestaus:</b>\nMarianne, Aaron, Iiris ja Aleksi" }
    },

    // Resources
    {"Energy", new string[]{"energy", "energia" } },
    {"Gold", new string[]{"gold", "kulta" } },
    {"Power", new string[]{"experience", "kokemusta" } },
    {"Life", new string[]{"life", "elinvoimaa" } },
    {"MaxLife", new string[]{"max life", "maksimielinvoimaa" } },
    {"damage", new string[]{"damage", "vahinkoa" } },
    {"heal", new string[]{"heal", "parane" } },
    {"RoomGold", new string[]{"gold in rooms", "kolikkoa huoneissa" } },
    {"RoomPower", new string[]{"experience in rooms", "kokemusta huoneissa" } },
    {"Aggro", new string[]{"monsters enrage speed", "hirviöiden raivostumisnopeus" } },

    // Pure gems
    {"pureRed", new string[]{"Red Gem", "Punainen jalokivi" } },
    {"pureRedDescription", new string[]{"Removes curses from surrounding gems and gives +1 additional attack when matched.", "Poistaa kiroukset ympäröivistä jalokivistä ja antaa yhden ylimääräisen hyökkäyksen särjettäessä." } },
    {"pureBlue", new string[]{"Blue Gem", "Sininen jalokivi" } },
    {"pureBlueDescription", new string[]{"Removes curses from surrounding gems and gives +1 additional defence when matched.", "Poistaa kiroukset ympäröivistä jalokivistä ja antaa yhden ylimääräisen puolustuksen särjettäessä." } },
    {"pureGreen", new string[]{"Green Gem", "Vihreä jalokivi" } },
    {"pureGreenDescription", new string[]{"Removes curses from surrounding gems and gives +1 additional energy when matched.", "Poistaa kiroukset ympäröivistä jalokivistä ja antaa yhden ylimääräisen energian särjettäessä." } },
    {"pureYellow", new string[]{"Yellow Gem", "Keltainen jalokivi" } },
    {"pureYellowDescription", new string[]{"Removes curses from surrounding gems and gives +2 additional gold when matched.", "Poistaa kiroukset ympäröivistä jalokivistä ja antaa kaksi ylimääräistä kultaa särjettäessä." } },
    {"pureGray", new string[]{"Gray Gem", "Harmaa jalokivi" } },
    {"pureGrayDescription", new string[]{"Removes curses from surrounding gems and gives +2 additional experience when matched.", "Poistaa kiroukset ympäröivistä jalokivistä ja antaa kaksi ylimääräistä kokemusta särjettäessä." } },

    // Bomb gems
    {"bombRed", new string[]{"Red Bomb", "Punainen pommi" } },
    {"bombRedDescription", new string[]{"Explodes when matched. Gives +1 additional attack.", "Räjähtää särjettäessä ja antaa +1 hyökkäyksen." } },
    {"bombBlue", new string[]{"Blue Bomb", "Sininen pommi" } },
    {"bombBlueDescription", new string[]{"Explodes when matched. Gives +1 additional energy.", "Räjähtää särjettäessä ja antaa +1 puolustusta." } },
    {"bombGreen", new string[]{"Green Bomb", "Vihreä pommi" } },
    {"bombGreenDescription", new string[]{"Explodes when matched. Gives +1 additional energy.", "Räjähtää särjettäessä ja antaa +1 energiaa." } },
    {"bombYellow", new string[]{"Yellow Bomb", "Keltainen pommi" } },
    {"bombYellowDescription", new string[]{"Explodes when matched. Gives +1 additional gold.", "Räjähtää särjettäessä ja antaa +2 kultaa." } },
    {"bombGray", new string[]{"Gray Bomb", "Harmaa pommi" } },
    {"bombGrayDescription", new string[]{"Explodes when matched. Gives +2 additional experience.", "Räjähtää särjettäessä ja antaa +2 kokemusta." } },

    //Slider gems
    {"sliderRed", new string[]{"Red Slider", "Punainen liukuja" } },
    {"sliderRedDescription", new string[]{"Slides until matches or collides with border. Gives +1 additional attack when matched.", "Siirrettäessä liukuu kunnes tuhoutuu tai törmää reunaan. Antaa +1 hyökkäyksen hajotessaan." } },
    {"sliderBlue", new string[]{"Blue Slider", "Sininen liukuja" } },
    {"sliderBlueDescription", new string[]{"Slides until matches or collides with border. Gives +1 additional defence when matched.", "Siirrettäessä liukuu kunnes tuhoutuu tai törmää reunaan. Antaa +1 puolustuksen hajotessaan." } },
    {"sliderGreen", new string[]{"Green Slider", "Vihreä liukuja" } },
    {"sliderGreenDescription", new string[]{"Slides until matches or collides with border. Gives +1 additional energy when matched.", "Siirrettäessä liukuu kunnes tuhoutuu tai törmää reunaan. Antaa +1 energian hajotessaan." } },
    {"sliderYellow", new string[]{"Yellow Slider", "Keltainen liukuja" } },
    {"sliderYellowDescription", new string[]{"Slides until matches or collides with border. Gives +2 additional gold when matched.", "Siirrettäessä liukuu kunnes tuhoutuu tai törmää reunaan. Antaa +2 kultaa hajotessaan." } },
    {"sliderGray", new string[]{"Gray Slider", "Harmaa liukuja" } },
    {"sliderGrayDescription", new string[]{"Slides until matches or collides with border. Gives +2 additional experience when matched.", "Siirrettäessä liukuu kunnes tuhoutuu tai törmää reunaan. Antaa +2 kokemusta hajotessaan." } },

    // Abilities
    {"abilityAction", new string[]{"Action", "Toiminta" } },
    {"abilityActionDescription", new string[]{"Gain one extra action.", "Saat ylimääräisen siirron." } },
    {"abilityAttack", new string[]{"Attack", "Hyökkäys" } },
    {"abilityAttackDescription", new string[]{"Gain one attack.", "Saat yhden hyökkäyksen." } },
    {"abilityGold", new string[]{"Thief", "Varas" } },
    {"abilityGoldDescription", new string[]{"Collect one gold from the room.", "Keräät yhden kolikon huoneesta." } },
    {"abilityGoldExtra", new string[]{"Collector", "Keräilijä" } },
    {"abilityGoldExtraDescription", new string[]{"Gain one extra gold.", "Saat yhden ylimääräisen kolikon." } },
    {"abilityUseGoldToken", new string[]{"Trap", "Ansa" } },
    {"abilityUseGoldTokenDescription", new string[]{"Use one coin token from the room to gain one attack, defence and energy.", "Kulutat yhden kolikon huoneesta saadaksesi yhden hyökkäyksen, puolustuksen ja energian." } },
    {"abilityHeal", new string[]{"Heal", "Parannus" } },
    {"abilityHealDescription", new string[]{"Heal one life.", "Paranet yhden elinvoiman." } },
    {"abilityPower", new string[]{"Fast learner", "Nopea oppija" } },
    {"abilityPowerDescription", new string[]{"Collect one experience from the room", "Keräät kokemuksen huoneesta." } },
    {"abilityPowerExtra", new string[]{"Insight", "Viisaus" } },
    {"abilityPowerExtraDescription", new string[]{"Gain one extra experience.", "Saat yhden ylimääräisen kokemuksen." } },
    {"abilityUsePowerToken", new string[]{"Soulburn", "Sielutuli" } },
    {"abilityUsePowerTokenDescription", new string[]{ "Use one experience token from the room to gain one attack, defence and energy.", "Kulutat yhden kokemuksen huoneesta saadaksesi yhden hyökkäyksen, puolustuksen ja energian." } },
    {"abilityDefend", new string[]{"Defend", "Puolustus" } },
    {"abilityDefendDescription", new string[]{"Gain one defence.", "Saat yhden puolustuksen." } },
    {"abilityCleanse", new string[]{"Cleanse", "Puhdistus" } },
    {"abilityCleanseDescription", new string[]{"Remove curse from two random cursed gems.", "Poistaa kirouksen kahdesta satunnaisesta jalokivestä." } },
    {"abilityShuffle", new string[]{"Shuffle", "Sekoitus" } },
    {"abilityShuffleDescription", new string[]{"Replaces gem board with new gems.", "Täyttää pelikentän uusilla jalokivillä." } },
    {"abilityDefenceFromAttack", new string[]{"Defencive stance", "Kilpikonna" } },
    {"abilityDefenceFromAttackDescription", new string[]{"Transforms all attack to defence.", "Muuntaa hyökkäyksen puolustukseksi." } },
    {"abilityAttackFromDefence", new string[]{"Attack stance", "Raivo" } },
    {"abilityAttackFromDefenceDescription", new string[]{ "Transforms all defence to attack.", "Muuntaa puolustuksen hyökkäykseksi." } },
    {"abilityEnergyFromDefence", new string[]{"Burst of speed", "Yllätysliike" } },
    {"abilityEnergyFromDefenceDescription", new string[]{ "Transforms all defence to energy.", "Muuntaa puolustuksen energiaksi." } },
    {"abilityBomb", new string[]{"Bomb", "Pommi" } },
    {"abilityBombDescription", new string[]{"Next gem match explodes.", "Seuraava yhdistetty jalokivi räjähtää." } },
    {"abilityFreeMove", new string[]{"Free move", "Vapaa siirto" } },
    {"abilityFreeMoveDescription", new string[]{"Next move, that doesn't break gems, wont cost an action.", "Seuraava siirto, joka ei riko jalokiviä, ei maksa vuoroa." } },
    {"abilitySlide", new string[]{"Slider", "Liukuja" } },
    {"abilitySlideDescription", new string[]{"Next moved gem slides.", "Seuraava siirretty jalokivi liukuu" } },
    {"abilityGoldToPower", new string[]{"Gold is knowledge", "Kulta on tietoa" } },
    {"abilityGoldToPowerDescription", new string[]{"Pay one gold to gain one extra experience.", "Maksa yksi kulta saadaksesi yhden ylimääräisen kokemuksen." } },
    {"abilityGoldToLife", new string[]{"Gold is Health", "Kulta on terveyttä" } },
    {"abilityGoldToLifeDescription", new string[]{"Pay two gold to heal one damage.", "Maksa kaksi kultaa parantuaksesi yhden vahingon." } },
    {"abilityGoldToEnergy", new string[]{"Gold is Energy", "Kulta on energiaa" } },
    {"abilityGoldToEnergyDescription", new string[]{"Pay one gold to gain one energy.", "Maksa yksi kulta saadaksesi yhden energian." } },
    {"abilityAttackFromLife", new string[]{"Rage", "Raivo" } },
    {"abilityAttackFromLifeDescription", new string[]{"Lose one life to gain two attack.", "Kärsi yksi vahinko saadaksesi kaksi hyökkäystä." } },
    {"abilityHealFromGold", new string[]{"Rebuild", "Korjautuminen" } },
    {"abilityHealFromGoldDescription", new string[]{"Use one gold token to heal one life.", "Käytä yksi kulta huoneesta parantuaksesi yhden vahingon." } },
    {"abilityCoinAdd", new string[]{"Mine gold", "Kullan kaivaminen" } },
    {"abilityCoinAddDescription", new string[]{"Adds one gold to the room.", "Käytettäessä lisää yhden kullan huoneeseen." } },
    {"abilityTriggerBreak", new string[]{"Break gem", "Jalokiven särkijä" } },
    {"abilityTriggerBreakDescription", new string[]{"Break next gem you touch.", "Rikkoo seuraavan jalokiven johon kosket." } },
    {"abilityPotential", new string[]{"Potential", "Itsevarmuus" } },
    {"abilityPotentialDescription", new string[]{"Gain one potential.", "Saat yhden itsevarmuutta." } },

    // Items
    {"itemGoldBag", new string[]{"Gold bag", "Kultapussi" } },
    {"itemGoldBagDescription", new string[]{"Rooms have one extra gold and you gain one extra gold when you encounter a monster.", "Huoneissa on yksi kulta enemmän ja saat yhden ylimääräisen kolikon aina kun kohtaat hirviön." } },
    {"itemGoldDamage", new string[]{"Golden blood", "Kultainen veri" } },
    {"itemGoldDamageDescription", new string[]{"Gain two extra gold when enemy damages you.", "Saat kaksi ylimääräistä kolikkoa aina kun vihollinen vahingoittaa sinua" } },
    {"itemPowerDamage", new string[]{"Experience collector", "Kokemuksen kerääjä" } },
    {"itemPowerDamageDescription", new string[]{"Gain one extra experience when enemy damages you.", "Saat yhden ylimääräisen kokemuksen aina kun vihollinen vahingoittaa sinua." } },
    {"itemSoulJar", new string[]{"Soulstone", "Sielukivi" } },
    {"itemSoulJarDescription", new string[]{"Roomas have one extra experience and you gain one extra experience when you encounter a monster.", "Huoneissa on yksi kokemus enemmän ja saat yhden ylimääräisen kokemuksen aina kun kohtaat hirviön." } },
    {"itemAggroReduction", new string[]{"Ring of peace", "Rauhansormus" } },
    {"itemAggroReductionDescription", new string[]{"Monsters enrage one round slower.", "Viholliset raivostuvat yhden vuoron hitaammin." } },
    {"itemAttackMinor", new string[]{"Knife", "Veitsi" } },
    {"itemAttackMinorDescription", new string[]{"At the start of combat gain one attack if you have none.", "Saat yhden hyökkäyksen taistelun aluksi, jos hyökkäyksesi on nolla." } },
    {"itemAttackNoDefence", new string[]{"Ring of rage", "Raivosormus" } },
    {"itemAttackNoDefenceDescription", new string[]{"Gain two attack if you don't have any defence.", "Saat kaksi hyökkäystä, jos sinulla ei ole yhtään puolustusta." } },
    {"itemAttackLowLife", new string[]{"Bloodstone", "Verikivi" } },
    {"itemAttackLowLifeDescription", new string[]{"At the start of round you gain one attack if you have no more than six life remaining.", "Saat kierroksen alussa yhden hyökkäyksen, jos sinulla on enintään kuusi elinvoimaa." } },
    {"itemDefenceNoAttack", new string[]{"Peace rune", "Rauhan symboli" } },
    {"itemDefenceNoAttackDescription", new string[]{"Gain two defence if you have no attack.", "Saat kaksi puolustusta, jos sinulla ei ole yhtään hyökkäystä." } },
    {"itemArmor", new string[]{"Armor", "Panssari" } },
    {"itemArmorDescription", new string[]{"At the end of round your defence is reduced only by two.", "Kierroksen päätteeksi menetät enintään 2 puolustusta." } },
    {"itemAttackDecay", new string[]{"Rage talisman", "Raivon talismaani" } },
    {"itemAttackDecayDescription", new string[]{"At the end of round your attack is reduced only by two.", "Kierroksen päätteeksi menetät enintään 2 hyökkäystä." } },
    {"itemDefenceLowLife", new string[]{"Ring of protection", "Suojauksen sormus" } },
    {"itemDefenceLowLifeDescription", new string[]{"Gain one defence if you have no more than six life remaining.", "Saat yhden panssarin jos sinulla on enintään kuusi elinvoimaa jäljellä." } },
    {"itemDefenceLowLife2", new string[]{"Ring of greater protection", "Suojauksen mahtisormus" } },
    {"itemDefenceLowLife2Description", new string[]{"Gain two defence at the start of combat phase, if you have no more than three life remaining.", "Saat kaksi panssaria taisteluvaiheen alussa, jos sinulla on enintään kolme elinvoimaa jäljellä." } },
    {"itemEnergyLowLife", new string[]{"Fear stone", "Pelkokivi" } },
    {"itemEnergyLowLifeDescription", new string[]{"At the start of round you gain two energy if you have no more than five life remaining.", "Saat kierroksen alussa kaksi energiaa, jos sinulla on enintään viisi elinvoimaa." } },
    {"itemDefenceMinor", new string[]{"Shield", "Kilpi" } },
    {"itemDefenceMinorDescription", new string[]{"Gain one defence at the start of combat if you have none.", "Saat yhden panssarin, jos sinulla ei ole panssaria taistelun alussa." } },
    {"itemEnergyEndless", new string[]{"Energy ring", "Energiasormus" } },
    {"itemEnergyEndlessDescription", new string[]{"At the start of round gain one enrgy if you have none.", "Saat yhden energian kierroksen aluksi, jos energiasi on nolla." } },
    {"itemEnergyStart", new string[]{"Magic potion", "Taikajuoma" } },
    {"itemEnergyStartDescription", new string[]{"Start each room with two additional energy.", "Aloitat huoneen kahdella ylimääräisellä energialla." } },
    {"itemGoldSmall", new string[]{"Bronze coins", "Pronssikolikoita" } },
    {"itemGoldSmallDescription", new string[]{"Gain 8 gold.", "Saat 8 kultaa." } },
    {"itemGoldMedium", new string[]{"Silver coins", "Hopeakolikoita" } },
    {"itemGoldMediumDescription", new string[]{"Gain 12 gold.", "Saat 12 kultaa." } },
    {"itemGoldLarge", new string[]{"Gold coins", "Kultakolikoita" } },
    {"itemGoldLargeDescription", new string[]{"Gain 15 gold.", "Saat 15 kultaa." } },
    {"itemGoldEnergy", new string[]{"Coin of greed", "Ahneuden kolikko" } },
    {"itemGoldEnergyDescription", new string[]{"Gain one energy at the start of round if there is no coins left in the room.", "Saat kierroksen alussa yhden energian, jos huoneessa ei ole kolikoita jäljellä." } },
    {"itemGoldTokens", new string[]{"Lockpicks", "Tiirikat" } },
    {"itemGoldTokensDescription", new string[]{"Each room has two extra gold tokens.", "Jokaisessa taistelussa on kaksi ylimääräistä kultaa saatavilla." } },
    {"itemHealingRoomStart", new string[]{"Ring of regeneration", "Parantamisen sormus" } },
    {"itemHealingRoomStartDescription", new string[]{"Heal one damage at the start of every room.", "Paranet yhden elinvoiman jokaisen huoneen alussa." } },
    {"itemHealingPotion", new string[]{"Potion of healing", "Parannusjuoma" } },
    {"itemHealingPotionDescription", new string[]{"Heal all damage.", "Parantaa kaiken vahingon." } },
    {"itemMaxLife", new string[]{"Elixir", "Eliksiiri" } },
    {"itemMaxLifeDescription", new string[]{"Your maximum life is increased by five.", "Lisää maksimielinvoimaasi viidellä pisteellä." } },
    {"itemBookSmall", new string[]{"Small book", "Kirja" } },
    {"itemBookSmallDescription", new string[]{"Gain four experience.", "Saat 4 kokemusta." } },
    {"itemBookMedium", new string[]{"Book", "Kirja" } },
    {"itemBookMediumDescription", new string[]{"Gain 6 experience.", "Saat 6 kokemusta." } },
    {"itemBookLarge", new string[]{"Large book", "Suuri kirja" } },
    {"itemBookLargeDescription", new string[]{"Gain 8 experience", "Saat 8 kokemusta." } },
    {"itemPowerTokens", new string[]{"Notebook", "Muistikirja " } },
    {"itemPowerTokensDescription", new string[]{"Each room has two extra experience tokens.", "Jokaisessa taistelussa on kaksi ylimääräistä kokemusta saatavilla." } },
    {"itemRageAttack", new string[]{"Rage potion", "Raivojuoma" } },
    {"itemRageAttackDescription", new string[]{"Gain two attack when enemy damages you.", "Saat kaksi hyökkäystä aina kun vihollinen vahingoittaa sinua." } },
    {"itemRageEnergy", new string[]{"Adrenaline potion", "Vauhtijuoma" } },
    {"itemRageEnergyDescription", new string[]{"Gain two energy when enemy damages you.", "Saat kaksi energiaa aina kun vihollinen vahingoittaa sinua." } },
    {"itemShopPrice", new string[]{"Old contract", "Vanha sopimus" } },
    {"itemShopPriceDescription", new string[]{"Chests are 30% cheaper to buy.", "Arkut ovat 30% halvempia ostaa." } },
    {"itemRageDefence", new string[]{"Stone potion", "Kivijuoma" } },
    {"itemRageDefenceDescription", new string[]{"Gain three defence when enemy damages you.", "Saat kolme panssaria aina kun vihollinen satuttaa sinua." } },
    {"itemGoldGemEmptyChance", new string[]{"Golden orb", "Kultainen pallo" } },
    {"itemGoldGemEmptyChanceDescription", new string[]{"Yellow gems don't appear if there is no gold left in the room.", "Keltaisia jalokiviä ei ilmesty, jos huoneessa ei ole kultaa jäljellä." } },
    {"itemPowerGemEmptyChance", new string[]{"Dream orb", "Unien pallo" } },
    {"itemPowerGemEmptyChanceDescription", new string[]{"White gems don't appear if there is no experience left in the room.", "Valkoisia jalokiviä ei ilmesty, jos huoneessa ei ole kokemusta jäljellä." } },
    {"itemBlueGemWeight", new string[]{"Blue rock", "Sininen kivi" } },
    {"itemBlueGemWeightDescription", new string[]{"+50% chance for blue gems to spawn.", "+50% sinisten jalokivien ilmestymismahdollisuuteen." } },
    {"itemRedGemWeight", new string[]{"Red rock", "Punainen kivi" } },
    {"itemRedGemWeightDescription", new string[]{"+50% chance for red gems to spawn.", "+50% punaisten jalokivien ilmestymismahdollisuuteen." } },
    {"itemLifeDrain", new string[]{"Blood drinker", "Veren juoja" } },
    {"itemLifeDrainDescription", new string[]{"Heal one life when you damage a monster. -10 maximum life.", "Paranet yhden elinvoiman kun vahingoitat hirviötä. -10 maksimielinvoimaa." } },
    {"itemEnergyDrain", new string[]{"Victory bracelets", "Voiton rannesuojat" } },
    {"itemEnergyDrainDescription", new string[]{"Gain one energy when you damage a monster.", "Saat yhden energian kun vahingoitat hirviötä." } },
    {"attackEmptyPower", new string[]{"Witch hat", "Noidan hattu" } },
    {"attackEmptyPowerDescription", new string[]{"Gain one attack at the start of round, if there is no experience left in the room.", "Saat yhden hyökkäyksen kierroksen aluksi, jos huoneessa ei ole kokemusta jäljellä." } },
    {"itemMaxLifeDrain", new string[]{"Blood axe", "Verikirves" } },
    {"itemMaxLifeDrainDescription", new string[]{"Gain one maximum life when you damage enemy with attack five or greater.", "Saat yhden maksimielinvoimaa kun vahingoitat vihollista vähintään viiden hyökkäyksellä." } },
    {"itemHealingSmall", new string[]{"Healing potion", "Parannusjuoma" } },
    {"itemHealingSmallDescription", new string[]{"Heal's five life.", "Parantaa viisi elinvoimaa." } },
    {"itemEnergyShield", new string[]{"Energy Shield", "Energiasuoja" } },
    {"itemEnergyShieldDescription", new string[]{"Your energy protects you from attacks.", "Energiasi suojelee sinua hyökkäyksiä vastaan." } },
    {"itemBloodEnergy", new string[]{"Blood magic", "Verimagia" } },
    {"itemBloodEnergyDescription", new string[]{"You can use life as energy.", "Voit käyttää elinvoimaasi energiana." } },
    {"itemLifeGrowth", new string[]{"Crystal of growth", "Kasvun kristalli" } },
    {"itemLifeGrowthDescription", new string[]{"Gain two maximum life when you gain a level, but your starting energy from hero level is halved.", "Saat kaksi maksimielinvoimaa kun saat sankaritason, mutta tasoista saamasi energia on puolitettu." } },
    {"itemRageAction", new string[]{"Ring of revenge", "Kaunasormus" } },
    {"itemRageActionDescription", new string[]{"You gain extra action when enemy damages you.", "Saat ylimääräisen siirron kun vihollinen vahingoittaa sinua." } },
    {"itemLifeFromDefence", new string[]{"Guardian shield", "Puolustajan kilpi" } },
    {"itemLifeFromDefenceDescription", new string[]{"Heal one if you have at least five defence when combat starts.", "Paranet yhden elinvoiman jos sinulla on vähintään viisi puolustusta taistelun alussa." } },
    {"itemGemBoardWidth", new string[]{"Gem Tome", "Jalokivikirja" } },
    {"itemGemBoardWidthDescription", new string[]{"Gem board width is increased by one.", "Jalokivikenttä kasvaa yhden rivin leveämmäksi." } },
    {"itemGemBoardHeight", new string[]{"Gem Tome", "Jalokivikirja" } },
    {"itemGemBoardHeightDescription", new string[]{"Gem board height is increased by one.", "Jalokivikenttä kasvaa yhden rivin korkeammaksi." } },
    {"itemMaxLifeDevour", new string[]{"Devourer", "Ahmija" } },
    {"itemMaxLifeDevourDescription", new string[]{"When you defeate an enemy, your maximum life is increased by remaining life that monster had.", "Kun voitat vihollisen, maksimielinvoimasi kasvaa vihollisen jäljellä olleen elinvoiman verran." } },
    {"itemBoardHeight", new string[]{"Book of Knowledge", "Tiedon kirja" } },
    {"itemBoardHeightDescription", new string[]{"Increases gem board height by one row.", "Suurentaa jalokivikenttää yhden rivin korkeammaksi." } },
    {"itemBoardWidth", new string[]{"Book of Secrets", "Salaisuuksien kirja" } },
    {"itemBoardWidthDescription", new string[]{"Increases gem board width by one column.", "Suurentaa jalokivikenttää yhden rivin leveämmäksi." } },
    {"itemGoldEndless", new string[]{"Bag of coins", "Kolikkopussi" } },
    {"itemGoldEndlessDescription", new string[]{"Lose all your gold. At the start of every round one coin appears to the room", "Menetät kaiken kultasi. Jokaisen kierroksen alussa huoneeseen ilmestyy uusi kolikko." } },
    {"itemEnergyFullLife", new string[]{"Lucky coin", "Onnenlantti" } },
    {"itemEnergyFullLifeDescription", new string[]{"Gain one energy each turn if you are at full life", "Saat jokaisen kierroksen alussa energian jos elinvoimasi on täynnä." } },
    {"itemHealingOnBlock", new string[]{"Shield of Regeneration", "Parantumisen kilpi" } },
    {"itemHealingOnBlockDescription", new string[]{"Heal one life when you block enemy attack.", "Paranet yhden elinvoiman kun pysäytät vihollisen hyökkäyksen." } },
    {"itemBlockPotential", new string[]{"Ring of opportunity", "Mahdollisuuksien sormus" } },
    {"itemBlockPotentialDescription", new string[]{"Gain one potential when you block enemy attack.", "Saat yhden itsevarmuutta kun pysäytät vihollisen hyökkäyksen." } },
    {"itemHealingHighAttack", new string[]{"Demon bracers", "Demonirannekkeet" } },
    {"itemHealingHighAttackDescription", new string[]{"Heal one life if you have attack of five or greater.", "Paranet yhden elinvoiman jos hyökkäyksesi on viisi tai enemmän." } },

    {"itemPowerAttack", new string[]{"Dream blade", "Unien miekka" } },
    {"itemPowerAttackDescription", new string[]{"Gain one attack each round, if there is no experience left in the room. Rooms have one extra experience token.", "Saat yhden hyökkäyksen kierroksen aluksi, jos huoneessa ei ole kokemusta saatavilla. Huoneissa on yksi ylimääräinen kokemus saatavilla." } },
    {"itemPowerEnergy", new string[]{"Dream ring", "Unien sormus" } },
    {"itemPowerEnergyDescription", new string[]{"Gain one energy each round, if there is no experience left in the room. Rooms have one extra experience token.", "Saat yhden energian kierroksen aluksi, jos huoneessa ei ole kokemusta saatavilla. Huoneissa on yksi ylimääräinen kokemus saatavilla." } },
    {"itemPowerDefence", new string[]{"Dream gauntlet", "Unien hanska" } },
    {"itemPowerDefenceDescription", new string[]{"Gain one defence each round, if there is no experience left in the room. Rooms have two extra experience token.", "Saat yhden puolustuksen kierroksen aluksi, jos huoneessa ei ole kokemusta saatavilla. Huoneissa on kaksi ylimääräinen kokemus saatavilla." } },
    {"itemGoldAttack", new string[]{"Golden blade", "Kultainen miekka" } },
    {"itemGoldAttackDescription", new string[]{"Gain one attack each round, if there is no coins left in the room. Rooms have one extra coin token.", "Saat yhden hyökkäyksen kierroksen aluksi, jos huoneessa ei ole kolikoita saatavilla. Huoneissa on yksi ylimääräinen kolikko saatavilla." } },
    {"itemGoldEnergy2", new string[]{"Golden ring", "Kultainen sormus" } },
    {"itemGoldEnergy2Description", new string[]{"Gain one energy each round, if there is no coins left in the room. Rooms have one extra coin token.", "Saat yhden energian kierroksen aluksi, jos huoneessa ei ole kolikoita saatavilla. Huoneissa on yksi ylimääräinen kolikko saatavilla." } },
    {"itemGoldDefence", new string[]{"Golden gauntlet", "Kultainen hanska" } },
    {"itemGoldDefenceDescription", new string[]{"Gain one defence each round, if there is no coins left in the room. Rooms have two extra coin token.", "Saat yhden puolustuksen kierroksen aluksi, jos huoneessa ei ole kolikoita saatavilla. Huoneissa on kaksi ylimääräinen kolikkoa saatavilla." } },
    {"itemDamageTakenDouble", new string[]{"Ring of pain", "Kärsimyksen sormus" } },
    {"itemDamageTakenDoubleDescription", new string[]{"All damage taken effects trigger twice.", "Kaikki vahingon ottamisesta aktivoituvat esineet aktivoituvat kahdesti." } },
    {"itemRagePotential", new string[]{"Crown of Thorns", "Piikkien kruunu" } },
    {"itemRagePotentialDescription", new string[]{"Gain two potential when enemy damages you.", "Saat kaksi itsevarmuutta kun vihollinen vahingoittaa sinua." } },
    {"itemPotentialFromGray", new string[]{"Endless Wisdom", "Loputon viisaus" } },
    {"itemPotentialFromGrayDescription", new string[]{"When room has no experience left, you gain potential instead of power from white gems.", "Kun huoneessa ei ole kokemusta jäljellä, saat itsevarmuutta kokemuksen sijaan valkoisista jalokivistä." } },
    {"itemPotentialFromYellow", new string[]{"Endless Opportunities", "Loputtomat mahdollisuudet" } },
    {"itemPotentialFromYellowDescription", new string[]{"When room has no gold left, you gain potential instead of power from yellow gems.", "Kun huoneessa ei ole kultaa jäljellä, saat itsevarmuutta kullan sijaan keltaisista jalokivistä." } },
    {"itemPotential", new string[]{"Orb of storms", "Myrskyjen pallo" } },
    {"itemPotentialDescription", new string[]{"Gain one potential at the end of every round.", "Saat yhden itsevarmuuden jokaisen kierroksen lopuksi." } },

    // Character items
    {"itemSnailDefence", new string[]{"Snail shell", "Etanan kuori" } },
    {"itemSnailDefenceDescription", new string[]{"Gain one defence at the start of combat phase.", "Saat yhden puolustuksen taisteluvaiheen aluksi." } },
    {"itemSnailEnergy", new string[]{"Snail power", "Etana energia" } },
    {"itemSnailEnergyDescription", new string[]{"Gain one energy at the start of every round.", "Saat yhden energian jokaisen kierroksen alussa." } },
    {"itemAttackDamageCaused", new string[]{"Orb of rage", "Raivon kivi" } },
    {"itemAttackDamageCausedDescription", new string[]{"Gain attack equal to damage you caused to enemy.", "Saat hyökkäystä tekemäsi vahingon verran hyökkäyksen jälkeen." } },

    // Events
    {"eventTitle", new string[]{"Title", "Otsikko" } },
    {"eventDescription", new string[]{"Choose one:", "Valitse yksi:" } },
    {"eventRestTitle", new string[]{"Camp site", "Nuotio" } },
    {"eventRestDescription", new string[]{"You found peaceful place where you can rest for a moment. Choose one:", "Löysit turvallisen paikan, jossa levätä hetken. Valitse yksi:" } },

    {"eventLiquidGold", new string[]{"You find living liquid gold. In what form do you take it?", "Löydät elävää nestemäistä kultaa. Missä muodossa otat sen?" } },
    {"eventChaseGold", new string[]{"Follow golden light", "Seuraa kultaista valoa" } },
    {"eventChasePower", new string[]{"Follow purple light", "Seuraa violettia valoa" } },
    {"eventFightDragon", new string[]{"Fight the dragon", "Hyökkää lohikäärmeen kimppuun" } },

    };

  public static string GetTranslation(string key, Dictionary<string, string> options = null)
  {
    try
    {
      string[] values = dictionary[key];
      string str = values[(int)DataManager.instance.gameSettings.language];
      if (options != null)
      {
        foreach(string k in options.Keys)
        {
          str = str.Replace("%" + k + "%", options[k]);
        }
      }
      return str;
    }
    catch
    {
      return $"Translation not found! ({key})";
    }
  }

  public static string GetTranslation(string key, StringFormat format)
  {
    string str = GetTranslation(key); switch (format)
    {
      case StringFormat.normal:
        break;
      case StringFormat.capitalized:
        str = Capitalize(str);
        break;
      case StringFormat.upcase:
        str = str.ToUpper();
        break;
    }
    return str;
  }

  public static string Capitalize(string str)
  {
    return str.Substring(0, 1).ToUpper() + str.Substring(1).ToLower();
  }

  public static string TranslateResouce(ResourceAmount[] resources)
  {
    string str = "";
    for(int i = 0; i < resources.Length; i++)
    {
      ResourceAmount r = resources[i];
      r.amount = DataManager.instance.GetAmount(r);
      if (i != 0)
      {
        str += ", ";
      }
      if (r.resource == Resource.Life){
        // Damage
        if (r.amount < 0)
        {
          str += $"{-r.amount} {dictionary["damage"][(int)DataManager.instance.gameSettings.language]}";
        }
        // Heal
        else
        {
          str += $"{dictionary["heal"][(int)DataManager.instance.gameSettings.language]} {r.amount}";
        }
      }
      else
      {
        if (r.amount > 0)
        {
          str += "+";
        }
        str += $"{r.amount} {dictionary[r.resource.ToString()][(int)DataManager.instance.gameSettings.language]}";
      }

    }
    return str;
  }

}


public enum Language { english = 0, finnish = 1};
