﻿using System.Collections.Generic;
using System.Xml.Linq;

namespace SyntaxHighlight {
    public class ColorsFromName {
        static Dictionary<string, uint> _colors {
            get {
                return new Dictionary<string, uint>() {
                    {"abbey", 0xFF4c4f56},
                    {"acadia", 0xFF1b1404},
                    {"acapulco", 0xFF7cb0a1},
                    {"aero", 0xFF7cb9e8},
                    {"affair", 0xFF714693},
                    {"akaroa", 0xFFd4c4a8},
                    {"alabaster", 0xFFfafafa},
                    {"allports", 0xFF0076a3},
                    {"almond", 0xFFefdecd},
                    {"alpine", 0xFFaf8f2c},
                    {"alto", 0xFFdbdbdb},
                    {"aluminium", 0xFFa9acb6},
                    {"amaranth", 0xFFe52b50},
                    {"amazon", 0xFF3b7a57},
                    {"amber", 0xFFffbf00},
                    {"americano", 0xFF87756e},
                    {"amethyst", 0xFF9966cc},
                    {"amour", 0xFFf9eaf3},
                    {"amulet", 0xFF7b9f80},
                    {"anakiwa", 0xFF9de5ff},
                    {"anzac", 0xFFe0b646},
                    {"ao", 0xFF008000},
                    {"apache", 0xFFdfbe6f},
                    {"apple", 0xFF4fa83d},
                    {"apricot", 0xFFfbceb1},
                    {"aquamarine", 0xFF7fffd4},
                    {"arapawa", 0xFF110c6c},
                    {"armadillo", 0xFF433e37},
                    {"arrowtown", 0xFF948771},
                    {"arsenic", 0xFF3b444b},
                    {"artichoke", 0xFF8f9779},
                    {"ash", 0xFFc6c3b5},
                    {"asparagus", 0xFF87a96b},
                    {"asphalt", 0xFF130a06},
                    {"astra", 0xFFfaeab9},
                    {"astral", 0xFF327da0},
                    {"astronaut", 0xFF283a77},
                    {"atlantis", 0xFF97cd2d},
                    {"atoll", 0xFF0a6f75},
                    {"aubergine", 0xFF3b0910},
                    {"auburn", 0xFFa52a2a},
                    {"aureolin", 0xFFfdee00},
                    {"avocado", 0xFF568203},
                    {"axolotl", 0xFF4e6649},
                    {"azalea", 0xFFf7c8da},
                    {"aztec", 0xFF0d1c19},
                    {"azure", 0xFF007fff},
                    {"bahia", 0xFFa5cb0c},
                    {"bamboo", 0xFFda6304},
                    {"bandicoot", 0xFF858470},
                    {"barberry", 0xFFded717},
                    {"barossa", 0xFF44012d},
                    {"bastille", 0xFF292130},
                    {"bazaar", 0xFF98777b},
                    {"beaver", 0xFF9f8170},
                    {"beeswax", 0xFFfef2c7},
                    {"beige", 0xFFf5f5dc},
                    {"belgion", 0xFFadd8ff},
                    {"bermuda", 0xFF7dd8c6},
                    {"bianca", 0xFFfcfbf3},
                    {"bilbao", 0xFF327c14},
                    {"birch", 0xFF373021},
                    {"biscay", 0xFF1b3162},
                    {"bismark", 0xFF497183},
                    {"bisque", 0xFFffe4c4},
                    {"bistre", 0xFF3d2b1f},
                    {"bitter", 0xFF868974},
                    {"bittersweet", 0xFFfe6f5e},
                    {"bizarre", 0xFFeededa},
                    {"black", 0xFF000000},
                    {"blackberry", 0xFF4d0135},
                    {"blackcurrant", 0xFF32293a},
                    {"blond", 0xFFfaf0be},
                    {"blossom", 0xFFdcb4bc},
                    {"blue", 0xFF0000ff},
                    {"blueberry", 0xFF4f86f7},
                    {"bluebonnet", 0xFF1c1cf0},
                    {"blumine", 0xFF18587a},
                    {"blush", 0xFFde5d83},
                    {"bole", 0xFF79443b},
                    {"bombay", 0xFFafb1b8},
                    {"bone", 0xFFe3dac9},
                    {"bordeaux", 0xFF5c0120},
                    {"bossanova", 0xFF4e2a5a},
                    {"botticelli", 0xFFc7dde5},
                    {"boulder", 0xFF7a7a7a},
                    {"bouquet", 0xFFae809e},
                    {"bourbon", 0xFFba6f1e},
                    {"boysenberry", 0xFF873260},
                    {"bracken", 0xFF4a2a04},
                    {"brandy", 0xFFdec196},
                    {"brass", 0xFFb5a642},
                    {"bridesmaid", 0xFFfef0ec},
                    {"bronco", 0xFFaba196},
                    {"bronze", 0xFFcd7f32},
                    {"bronzetone", 0xFF4d400f},
                    {"broom", 0xFFffec13},
                    {"brown", 0xFF964b00},
                    {"bubbles", 0xFFe7feff},
                    {"buccaneer", 0xFF622f30},
                    {"bud", 0xFFa8ae9c},
                    {"buff", 0xFFf0dc82},
                    {"bunker", 0xFF0d1117},
                    {"bunting", 0xFF151f4c},
                    {"burgundy", 0xFF800020},
                    {"burlywood", 0xFFdeb887},
                    {"burnham", 0xFF002e20},
                    {"bush", 0xFF0d2e1c},
                    {"buttercup", 0xFFf3ad16},
                    {"buttermilk", 0xFFfff1b5},
                    {"byzantine", 0xFFbd33a4},
                    {"byzantium", 0xFF702963},
                    {"cabaret", 0xFFd94972},
                    {"cactus", 0xFF587156},
                    {"cadet", 0xFF536872},
                    {"cadillac", 0xFFb04c6a},
                    {"calico", 0xFFe0c095},
                    {"california", 0xFFfe9d04},
                    {"calypso", 0xFF31728d},
                    {"camarone", 0xFF00581a},
                    {"camelot", 0xFF893456},
                    {"cameo", 0xFFd9b99b},
                    {"camouflage", 0xFF3c3910},
                    {"canary", 0xFFf3fb62},
                    {"candlelight", 0xFFfcd917},
                    {"caper", 0xFFdcedb4},
                    {"capri", 0xFF00bfff},
                    {"caramel", 0xFFffddaf},
                    {"cararra", 0xFFeeeee8},
                    {"cardinal", 0xFFc41e3a},
                    {"carissma", 0xFFea88a8},
                    {"carla", 0xFFf3ffd8},
                    {"carmine", 0xFF960018},
                    {"carnation", 0xFFf95a61},
                    {"carnelian", 0xFFb31b1b},
                    {"casablanca", 0xFFf8b853},
                    {"casal", 0xFF2f6168},
                    {"cascade", 0xFF8ba9a5},
                    {"cashmere", 0xFFe6bea5},
                    {"casper", 0xFFadbed1},
                    {"castro", 0xFF52001f},
                    {"catawba", 0xFF703642},
                    {"cedar", 0xFF3e1c14},
                    {"ceil", 0xFF92a1cf},
                    {"celadon", 0xFFace1af},
                    {"celery", 0xFFb8c25d},
                    {"celeste", 0xFFb2ffff},
                    {"cello", 0xFF1e385b},
                    {"celtic", 0xFF163222},
                    {"cement", 0xFF8d7662},
                    {"ceramic", 0xFFfcfff9},
                    {"cerise", 0xFFde3163},
                    {"cerulean", 0xFF007ba7},
                    {"chablis", 0xFFfff4f3},
                    {"chalky", 0xFFeed794},
                    {"chambray", 0xFF354e8c},
                    {"chamois", 0xFFeddcb1},
                    {"chamoisee", 0xFFa0785a},
                    {"champagne", 0xFFf7e7ce},
                    {"chantilly", 0xFFf8c3df},
                    {"charade", 0xFF292937},
                    {"charcoal", 0xFF36454f},
                    {"chardon", 0xFFfff3f1},
                    {"chardonnay", 0xFFffcd8c},
                    {"charlotte", 0xFFbaeef9},
                    {"charm", 0xFFd47494},
                    {"chartreuse", 0xFFdfff00},
                    {"chatelle", 0xFFbdb3c7},
                    {"chenin", 0xFFdfcd6f},
                    {"cherokee", 0xFFfcda98},
                    {"cherrywood", 0xFF651a14},
                    {"cherub", 0xFFf8d9e9},
                    {"chestnut", 0xFF954535},
                    {"chicago", 0xFF5d5c58},
                    {"chiffon", 0xFFf1ffc8},
                    {"chino", 0xFFcec7a7},
                    {"chinook", 0xFFa8e3bd},
                    {"chocolate", 0xFF7b3f00},
                    {"christalle", 0xFF33036b},
                    {"christi", 0xFF67a712},
                    {"christine", 0xFFe7730a},
                    {"cinder", 0xFF0e0e18},
                    {"cinderella", 0xFFfde1dc},
                    {"cinereous", 0xFF98817b},
                    {"cinnabar", 0xFFe34234},
                    {"cioccolato", 0xFF55280c},
                    {"citrine", 0xFFe4d00a},
                    {"citron", 0xFF9fa91f},
                    {"citrus", 0xFFa1c50a},
                    {"clairvoyant", 0xFF480656},
                    {"claret", 0xFF7f1734},
                    {"clementine", 0xFFe96e00},
                    {"clinker", 0xFF371d09},
                    {"cloud", 0xFFc7c4bf},
                    {"cloudy", 0xFFaca59f},
                    {"clover", 0xFF384910},
                    {"coconut", 0xFF965a3e},
                    {"coffee", 0xFF6f4e37},
                    {"cognac", 0xFF9f381d},
                    {"cola", 0xFF3f2500},
                    {"comet", 0xFF5c5d75},
                    {"como", 0xFF517c66},
                    {"conch", 0xFFc9d9d2},
                    {"concord", 0xFF7c7b7a},
                    {"concrete", 0xFFf2f2f2},
                    {"confetti", 0xFFe9d75a},
                    {"conifer", 0xFFacdd4d},
                    {"contessa", 0xFFc6726b},
                    {"copper", 0xFFb87333},
                    {"coquelicot", 0xFFff3800},
                    {"coral", 0xFFff7f50},
                    {"cordovan", 0xFF893f45},
                    {"corduroy", 0xFF606e68},
                    {"coriander", 0xFFc4d0b0},
                    {"cork", 0xFF40291d},
                    {"corn", 0xFFe7bf05},
                    {"cornsilk", 0xFFfff8dc},
                    {"corvette", 0xFFfad3a2},
                    {"cosmic", 0xFF76395d},
                    {"cosmos", 0xFFffd8d9},
                    {"cowboy", 0xFF4d282d},
                    {"crail", 0xFFb95140},
                    {"cranberry", 0xFFdb5079},
                    {"cream", 0xFFfffdd0},
                    {"creole", 0xFF1e0f04},
                    {"crete", 0xFF737829},
                    {"crimson", 0xFFdc143c},
                    {"crocodile", 0xFF736d58},
                    {"crowshead", 0xFF1c1208},
                    {"cruise", 0xFFb5ecdf},
                    {"crusoe", 0xFF004816},
                    {"crusta", 0xFFfd7b33},
                    {"cumin", 0xFF924321},
                    {"cumulus", 0xFFfdffd5},
                    {"cupid", 0xFFfbbeda},
                    {"cyan", 0xFF00ffff},
                    {"cyclamen", 0xFFf56fa1},
                    {"cyprus", 0xFF003e40},
                    {"daffodil", 0xFFffff31},
                    {"daintree", 0xFF012731},
                    {"dallas", 0xFF6e4b26},
                    {"dandelion", 0xFFf0e130},
                    {"danube", 0xFF6093d1},
                    {"dawn", 0xFFa6a29a},
                    {"deco", 0xFFd2da97},
                    {"deer", 0xFFba8759},
                    {"dell", 0xFF396413},
                    {"delta", 0xFFa4a49d},
                    {"deluge", 0xFF7563a8},
                    {"denim", 0xFF1560bd},
                    {"derby", 0xFFffeed8},
                    {"desert", 0xFFae6020},
                    {"desire", 0xFFea3c53},
                    {"dew", 0xFFeafffe},
                    {"diamond", 0xFFb9f2ff},
                    {"diesel", 0xFF130000},
                    {"dingley", 0xFF5d7747},
                    {"dirt", 0xFF9b7653},
                    {"disco", 0xFF871550},
                    {"dixie", 0xFFe29418},
                    {"dogs", 0xFFb86d29},
                    {"dolly", 0xFFf9ff8b},
                    {"dolphin", 0xFF646077},
                    {"domino", 0xFF8e775e},
                    {"dorado", 0xFF6b5755},
                    {"downriver", 0xFF092256},
                    {"downy", 0xFF6fd0c5},
                    {"driftwood", 0xFFaf8751},
                    {"drover", 0xFFfdf7ad},
                    {"dune", 0xFF383533},
                    {"eagle", 0xFFb6baa4},
                    {"ebb", 0xFFe9e3e3},
                    {"ebony", 0xFF555d50},
                    {"eclipse", 0xFF311c17},
                    {"ecru", 0xFFc2b280},
                    {"ecstasy", 0xFFfa7814},
                    {"eden", 0xFF105852},
                    {"edgewater", 0xFFc8e3d7},
                    {"edward", 0xFFa2aeab},
                    {"eggplant", 0xFF614051},
                    {"eggshell", 0xFFf0ead6},
                    {"elephant", 0xFF123447},
                    {"elm", 0xFF1c7c7d},
                    {"emerald", 0xFF50c878},
                    {"eminence", 0xFF6c3082},
                    {"emperor", 0xFF514649},
                    {"empress", 0xFF817377},
                    {"endeavour", 0xFF0056a7},
                    {"envy", 0xFF8ba690},
                    {"equator", 0xFFe1bc64},
                    {"espresso", 0xFF612718},
                    {"eternity", 0xFF211a0e},
                    {"eucalyptus", 0xFF44d7a8},
                    {"eunry", 0xFFcfa39d},
                    {"everglade", 0xFF1c402e},
                    {"falcon", 0xFF7f626d},
                    {"fallow", 0xFFc19a6b},
                    {"fandango", 0xFFb53389},
                    {"fantasy", 0xFFfaf3f0},
                    {"fawn", 0xFFe5aa70},
                    {"fedora", 0xFF796a78},
                    {"feijoa", 0xFF9fdd8c},
                    {"feldgrau", 0xFF4d5d53},
                    {"fern", 0xFF63b76c},
                    {"ferra", 0xFF704f50},
                    {"festival", 0xFFfbe96c},
                    {"feta", 0xFFf0fcea},
                    {"finch", 0xFF626649},
                    {"finlandia", 0xFF556d56},
                    {"finn", 0xFF692d54},
                    {"fiord", 0xFF405169},
                    {"fire", 0xFFaa4203},
                    {"firebrick", 0xFFb22222},
                    {"firefly", 0xFF0e2a30},
                    {"flame", 0xFFe25822},
                    {"flamenco", 0xFFff7d07},
                    {"flamingo", 0xFFf2552a},
                    {"flavescent", 0xFFf7e98e},
                    {"flax", 0xFFeedc82},
                    {"flint", 0xFF6f6a61},
                    {"flirt", 0xFFa2006d},
                    {"foam", 0xFFd8fcfa},
                    {"fog", 0xFFd7d0ff},
                    {"folly", 0xFFff004f},
                    {"frangipani", 0xFFffdeb3},
                    {"froly", 0xFFf57584},
                    {"frost", 0xFFedf5dd},
                    {"frostbite", 0xFFe936a7},
                    {"frostee", 0xFFe4f6e7},
                    {"fuchsia", 0xFFff00ff},
                    {"fuego", 0xFFbede0d},
                    {"fulvous", 0xFFe48400},
                    {"gainsboro", 0xFFdcdcdc},
                    {"gallery", 0xFFefefef},
                    {"galliano", 0xFFdcb20c},
                    {"gamboge", 0xFFe49b0f},
                    {"geebung", 0xFFd18f1b},
                    {"genoa", 0xFF15736b},
                    {"geraldine", 0xFFfb8989},
                    {"geyser", 0xFFd4dfe2},
                    {"ghost", 0xFFc7c9d5},
                    {"gigas", 0xFF523c94},
                    {"gimblet", 0xFFb8b56a},
                    {"gin", 0xFFe8f2eb},
                    {"ginger", 0xFFb06500},
                    {"givry", 0xFFf8e4bf},
                    {"glacier", 0xFF80b3c4},
                    {"glaucous", 0xFF6082b6},
                    {"glitter", 0xFFe6e8fa},
                    {"goblin", 0xFF3d7d52},
                    {"golden", 0xFFffd700},
                    {"goldenrod", 0xFFdaa520},
                    {"gondola", 0xFF261414},
                    {"gorse", 0xFFfff14f},
                    {"gossamer", 0xFF069b81},
                    {"gossip", 0xFFd2f8b0},
                    {"gothic", 0xFF6d92a1},
                    {"grandis", 0xFFffd38c},
                    {"grape", 0xFF6f2da8},
                    {"graphite", 0xFF251607},
                    {"gravel", 0xFF4a444b},
                    {"gray", 0xFF808080},
                    {"green", 0xFF00ff00},
                    {"grenadier", 0xFFd54600},
                    {"grizzly", 0xFF885818},
                    {"grullo", 0xFFa99a86},
                    {"gumbo", 0xFF7ca1a6},
                    {"gunmetal", 0xFF2a3439},
                    {"gunsmoke", 0xFF828685},
                    {"gurkha", 0xFF9a9577},
                    {"hacienda", 0xFF98811b},
                    {"haiti", 0xFF1b1035},
                    {"hampton", 0xFFe5d8af},
                    {"harlequin", 0xFF3fff00},
                    {"harp", 0xFFe6f2ea},
                    {"heath", 0xFF541012},
                    {"heather", 0xFFb7c3d0},
                    {"heliotrope", 0xFFdf73ff},
                    {"hemlock", 0xFF5e5d3b},
                    {"hemp", 0xFF907874},
                    {"hibiscus", 0xFFb6316c},
                    {"highland", 0xFF6f8e63},
                    {"hillary", 0xFFaca586},
                    {"himalaya", 0xFF6a5d1b},
                    {"hoki", 0xFF65869f},
                    {"holly", 0xFF011d13},
                    {"honeydew", 0xFFf0fff0},
                    {"honeysuckle", 0xFFedfc84},
                    {"hopbush", 0xFFd06da1},
                    {"horizon", 0xFF5a87a0},
                    {"horses", 0xFF543d37},
                    {"hurricane", 0xFF877c7b},
                    {"husk", 0xFFb7a458},
                    {"iceberg", 0xFF71a6d2},
                    {"icterine", 0xFFfcf75e},
                    {"illusion", 0xFFf6a4c9},
                    {"imperial", 0xFF602f6b},
                    {"inchworm", 0xFFb2ec5d},
                    {"independence", 0xFF4c516d},
                    {"indigo", 0xFF4b0082},
                    {"indochine", 0xFFc26b03},
                    {"iris", 0xFF5a4fcf},
                    {"iroko", 0xFF433120},
                    {"iron", 0xFFd4d7d9},
                    {"ironstone", 0xFF86483c},
                    {"irresistible", 0xFFb3446c},
                    {"isabelline", 0xFFf4f0ec},
                    {"ivory", 0xFFfffff0},
                    {"jacaranda", 0xFF2e0329},
                    {"jacarta", 0xFF3a2a6a},
                    {"jade", 0xFF00a86b},
                    {"jaffa", 0xFFef863f},
                    {"jagger", 0xFF350e57},
                    {"jaguar", 0xFF080110},
                    {"jambalaya", 0xFF5b3013},
                    {"janna", 0xFFf4ebd3},
                    {"japonica", 0xFFd87c63},
                    {"jasmine", 0xFFf8de7e},
                    {"jasper", 0xFFd73b3e},
                    {"java", 0xFF1fc2c2},
                    {"jet", 0xFF343434},
                    {"jewel", 0xFF126b40},
                    {"jon", 0xFF3b1f1f},
                    {"jonquil", 0xFFf4ca16},
                    {"jumbo", 0xFF7c7b82},
                    {"juniper", 0xFF6d9292},
                    {"kabul", 0xFF5e483e},
                    {"kangaroo", 0xFFc6c8bd},
                    {"karaka", 0xFF1e1609},
                    {"karry", 0xFFffead4},
                    {"kelp", 0xFF454936},
                    {"keppel", 0xFF3ab09e},
                    {"khaki", 0xFFc3b091},
                    {"kidnapper", 0xFFe1ead4},
                    {"kilamanjaro", 0xFF240c02},
                    {"killarney", 0xFF3a6a47},
                    {"kimberly", 0xFF736c9f},
                    {"kobi", 0xFFe79fc4},
                    {"kobicha", 0xFF6b4423},
                    {"kokoda", 0xFF6e6d57},
                    {"korma", 0xFF8f4b0e},
                    {"koromiko", 0xFFffbd5f},
                    {"kournikova", 0xFFffe772},
                    {"kumera", 0xFF886221},
                    {"laser", 0xFFc8b568},
                    {"laurel", 0xFF749378},
                    {"lava", 0xFFcf1020},
                    {"lavender", 0xFFb57edc},
                    {"leather", 0xFF967059},
                    {"lemon", 0xFFfff700},
                    {"lenurple", 0xFFba93d8},
                    {"liberty", 0xFF545aa7},
                    {"licorice", 0xFF1a1110},
                    {"lilac", 0xFFc8a2c8},
                    {"lily", 0xFFc8aabf},
                    {"lima", 0xFF76bd17},
                    {"lime", 0xFFbfff00},
                    {"limeade", 0xFF6f9d02},
                    {"limerick", 0xFF9dc209},
                    {"linen", 0xFFfaf0e6},
                    {"lipstick", 0xFFab0563},
                    {"liver", 0xFF674c47},
                    {"loafer", 0xFFeef4de},
                    {"loblolly", 0xFFbdc9ce},
                    {"lochinvar", 0xFF2c8c84},
                    {"lochmara", 0xFF007ec7},
                    {"locust", 0xFFa8af8e},
                    {"logan", 0xFFaaa9cd},
                    {"lola", 0xFFdfcfdb},
                    {"lonestar", 0xFF6d0101},
                    {"lotus", 0xFF863c3c},
                    {"loulou", 0xFF460b41},
                    {"lucky", 0xFFaf9f1c},
                    {"lumber", 0xFFffe4cd},
                    {"lust", 0xFFe62020},
                    {"lynch", 0xFF697e9a},
                    {"mabel", 0xFFd9f7ff},
                    {"madang", 0xFFb7f0be},
                    {"madison", 0xFF09255d},
                    {"madras", 0xFF3f3002},
                    {"magenta", 0xFFca1f7b},
                    {"magnolia", 0xFFf8f4ff},
                    {"mahogany", 0xFFc04000},
                    {"maize", 0xFFfbec5d},
                    {"makara", 0xFF897d6d},
                    {"mako", 0xFF444954},
                    {"malachite", 0xFF0bda51},
                    {"malibu", 0xFF7dc8f7},
                    {"mallard", 0xFF233418},
                    {"malta", 0xFFbdb2a1},
                    {"mamba", 0xFF8e8190},
                    {"manatee", 0xFF979aaa},
                    {"mandalay", 0xFFad781b},
                    {"mandarin", 0xFFf37a48},
                    {"mandy", 0xFFe25465},
                    {"manhattan", 0xFFf5c999},
                    {"mantis", 0xFF74c365},
                    {"mantle", 0xFF8b9c90},
                    {"manz", 0xFFeeef78},
                    {"marigold", 0xFFeaa221},
                    {"mariner", 0xFF286acd},
                    {"maroon", 0xFF800000},
                    {"marshland", 0xFF0b0f08},
                    {"martini", 0xFFafa09e},
                    {"martinique", 0xFF363050},
                    {"marzipan", 0xFFf8db9d},
                    {"masala", 0xFF403b38},
                    {"matisse", 0xFF1b659d},
                    {"matrix", 0xFFb05d54},
                    {"matterhorn", 0xFF4e3b41},
                    {"mauve", 0xFFe0b0ff},
                    {"mauvelous", 0xFFef98aa},
                    {"maverick", 0xFFd8c2d5},
                    {"melanie", 0xFFe4c2d5},
                    {"melanzane", 0xFF300529},
                    {"melon", 0xFFfdbcb4},
                    {"melrose", 0xFFc7c1ff},
                    {"mercury", 0xFFe5e5e5},
                    {"merino", 0xFFf6f0e6},
                    {"merlin", 0xFF413c37},
                    {"merlot", 0xFF831923},
                    {"meteor", 0xFFd07d12},
                    {"meteorite", 0xFF3c1f76},
                    {"midnight", 0xFF702670},
                    {"mikado", 0xFF2d2510},
                    {"milan", 0xFFfaffa4},
                    {"millbrook", 0xFF594433},
                    {"mimosa", 0xFFf8fdd3},
                    {"mindaro", 0xFFe3f988},
                    {"ming", 0xFF36747d},
                    {"minsk", 0xFF3f307f},
                    {"mint", 0xFF3eb489},
                    {"mirage", 0xFF161928},
                    {"mischka", 0xFFd1d2dd},
                    {"mobster", 0xFF7f7589},
                    {"moccaccino", 0xFF6e1d14},
                    {"moccasin", 0xFFffe4b5},
                    {"mocha", 0xFF782d19},
                    {"mojo", 0xFFc04737},
                    {"monarch", 0xFF8b0723},
                    {"mondo", 0xFF4a3c30},
                    {"mongoose", 0xFFb5a27f},
                    {"monsoon", 0xFF8a8389},
                    {"monza", 0xFFc7031e},
                    {"mortar", 0xFF504351},
                    {"mosque", 0xFF036a6e},
                    {"muesli", 0xFFaa8b5b},
                    {"mulberry", 0xFFc54b8c},
                    {"mustard", 0xFFffdb58},
                    {"mystic", 0xFFd65282},
                    {"nandor", 0xFF4b5d52},
                    {"napa", 0xFFaca494},
                    {"narvik", 0xFFedf9f1},
                    {"navy", 0xFF000080},
                    {"nebula", 0xFFcbdbd6},
                    {"negroni", 0xFFffe2c5},
                    {"nepal", 0xFF8eabc1},
                    {"neptune", 0xFF7cb7bb},
                    {"nero", 0xFF140600},
                    {"nevada", 0xFF646e75},
                    {"niagara", 0xFF06a189},
                    {"nickel", 0xFF727472},
                    {"nobel", 0xFFb7b1b1},
                    {"nomad", 0xFFbab1a2},
                    {"norway", 0xFFa8bd9f},
                    {"nugget", 0xFFc59922},
                    {"nutmeg", 0xFF81422c},
                    {"nyanza", 0xFFe9ffdb},
                    {"oasis", 0xFFfeefce},
                    {"observatory", 0xFF02866f},
                    {"ochre", 0xFFcc7722},
                    {"oil", 0xFF281e15},
                    {"olive", 0xFF808000},
                    {"olivetone", 0xFF716e10},
                    {"olivine", 0xFF9ab973},
                    {"onahau", 0xFFcdf4ff},
                    {"onion", 0xFF2f270e},
                    {"onyx", 0xFF353839},
                    {"opal", 0xFFa9c6c2},
                    {"opium", 0xFF8e6f70},
                    {"oracle", 0xFF377475},
                    {"orange", 0xFFff7f00},
                    {"orchid", 0xFFda70d6},
                    {"oregon", 0xFF9b4703},
                    {"organ", 0xFF6c2e1f},
                    {"orient", 0xFF015e85},
                    {"orinoco", 0xFFf3fbd4},
                    {"ottoman", 0xFFe9f8ed},
                    {"oxley", 0xFF779e86},
                    {"paarl", 0xFFa65529},
                    {"pablo", 0xFF776f61},
                    {"pacifika", 0xFF778120},
                    {"paco", 0xFF411f10},
                    {"padua", 0xFFade6c4},
                    {"pampas", 0xFFf4f2ee},
                    {"panache", 0xFFeaf6ee},
                    {"pancho", 0xFFedcdab},
                    {"paprika", 0xFF8d0226},
                    {"paradiso", 0xFF317d82},
                    {"parchment", 0xFFf1e9d2},
                    {"parsley", 0xFF134f19},
                    {"patina", 0xFF639a8f},
                    {"paua", 0xFF260368},
                    {"pavlova", 0xFFd7c498},
                    {"peach", 0xFFffcba4},
                    {"peanut", 0xFF782f16},
                    {"pear", 0xFFd1e231},
                    {"pearl", 0xFFeae0c8},
                    {"peat", 0xFF716b56},
                    {"pelorous", 0xFF3eabbf},
                    {"peppermint", 0xFFe3f5e1},
                    {"perano", 0xFFa9bef2},
                    {"perfume", 0xFFd0bef8},
                    {"peridot", 0xFFe6e200},
                    {"periwinkle", 0xFFccccff},
                    {"persimmon", 0xFFec5800},
                    {"peru", 0xFFcd853f},
                    {"pesto", 0xFF7c7631},
                    {"pewter", 0xFF96a8a1},
                    {"pharlap", 0xFFa3807b},
                    {"picasso", 0xFFfff39d},
                    {"pink", 0xFFffc0cb},
                    {"piper", 0xFFc96323},
                    {"pipi", 0xFFfef4cc},
                    {"pippin", 0xFFffe1df},
                    {"pistachio", 0xFF93c572},
                    {"pizazz", 0xFFff9000},
                    {"pizza", 0xFFc99415},
                    {"plantation", 0xFF27504b},
                    {"platinum", 0xFFe5e4e2},
                    {"plum", 0xFF8e4585},
                    {"pohutukawa", 0xFF8f021c},
                    {"polar", 0xFFe5f9f6},
                    {"pomegranate", 0xFFf34723},
                    {"pompadour", 0xFF660045},
                    {"popstar", 0xFFbe4f62},
                    {"porcelain", 0xFFeff2f3},
                    {"porsche", 0xFFeaae69},
                    {"portafino", 0xFFffffb4},
                    {"portage", 0xFF8b9fee},
                    {"portica", 0xFFf9e663},
                    {"prelude", 0xFFd0c0e5},
                    {"prim", 0xFFf0e2ec},
                    {"primrose", 0xFFedea99},
                    {"puce", 0xFFcc8899},
                    {"pueblo", 0xFF7d2c14},
                    {"pumice", 0xFFc2cac4},
                    {"pumpkin", 0xFFff7518},
                    {"punch", 0xFFdc4333},
                    {"punga", 0xFF4d3d14},
                    {"purple", 0xFF800080},
                    {"purpureus", 0xFF9a4eae},
                    {"putty", 0xFFe7cd8c},
                    {"quartz", 0xFF51484f},
                    {"quicksand", 0xFFbd978e},
                    {"quincy", 0xFF623f2d},
                    {"raffia", 0xFFeadab8},
                    {"rainee", 0xFFb9c8ac},
                    {"rajah", 0xFFfbab60},
                    {"rangitoto", 0xFF2e3222},
                    {"raspberry", 0xFFe30b5d},
                    {"raven", 0xFF727b89},
                    {"razzmatazz", 0xFFe3256b},
                    {"rebel", 0xFF3c1206},
                    {"red", 0xFFff0000},
                    {"redwood", 0xFFa45a52},
                    {"reef", 0xFFc9ffa2},
                    {"regalia", 0xFF522d80},
                    {"remy", 0xFFfeebf3},
                    {"revolver", 0xFF2c1632},
                    {"rhino", 0xFF2e3f62},
                    {"rhythm", 0xFF777696},
                    {"riptide", 0xFF8be6d8},
                    {"rock", 0xFF4d3833},
                    {"roman", 0xFFde6360},
                    {"romance", 0xFFfffefd},
                    {"romantic", 0xFFffd2b7},
                    {"ronchi", 0xFFecc54e},
                    {"rope", 0xFF8e4d1e},
                    {"rose", 0xFFff007f},
                    {"rosewood", 0xFF65000b},
                    {"roti", 0xFFc6a84b},
                    {"rouge", 0xFFa23b6c},
                    {"ruber", 0xFFce4676},
                    {"ruby", 0xFFe0115f},
                    {"ruddy", 0xFFff0028},
                    {"rufous", 0xFFa81c07},
                    {"rum", 0xFF796989},
                    {"russet", 0xFF80461b},
                    {"russett", 0xFF755a57},
                    {"rust", 0xFFb7410e},
                    {"saddle", 0xFF4c3024},
                    {"saffron", 0xFFf4c430},
                    {"sage", 0xFFbcb88a},
                    {"sahara", 0xFFb7a214},
                    {"sail", 0xFFb8e0f9},
                    {"salem", 0xFF097f4b},
                    {"salmon", 0xFFfa8072},
                    {"salomie", 0xFFfedb8d},
                    {"saltpan", 0xFFf1f7f2},
                    {"sambuca", 0xFF3a2010},
                    {"sandal", 0xFFaa8d6f},
                    {"sandrift", 0xFFab917a},
                    {"sandstone", 0xFF796d62},
                    {"sandstorm", 0xFFecd540},
                    {"sandwisp", 0xFFf5e7a2},
                    {"sangria", 0xFF92000a},
                    {"sapling", 0xFFded4a4},
                    {"sapphire", 0xFF0f52ba},
                    {"saratoga", 0xFF555b10},
                    {"sauvignon", 0xFFfff5f3},
                    {"sazerac", 0xFFfff4e0},
                    {"scampi", 0xFF675fa6},
                    {"scandal", 0xFFcffaf4},
                    {"scarlet", 0xFFff2400},
                    {"scarlett", 0xFF950015},
                    {"schist", 0xFFa9b497},
                    {"schooner", 0xFF8b847e},
                    {"scooter", 0xFF2ebfd4},
                    {"scorpion", 0xFF695f62},
                    {"seagull", 0xFF80ccea},
                    {"seance", 0xFF731e8f},
                    {"seashell", 0xFFfff5ee},
                    {"seaweed", 0xFF1b2f11},
                    {"selago", 0xFFf0eefd},
                    {"sepia", 0xFF704214},
                    {"serenade", 0xFFfff4e8},
                    {"shadow", 0xFF8a795d},
                    {"shakespeare", 0xFF4eabd1},
                    {"shalimar", 0xFFfbffba},
                    {"shampoo", 0xFFffcff1},
                    {"shamrock", 0xFF33cc99},
                    {"shark", 0xFF25272c},
                    {"shilo", 0xFFe8b9b3},
                    {"shiraz", 0xFFb20931},
                    {"shocking", 0xFFe292c0},
                    {"siam", 0xFF646a54},
                    {"sidecar", 0xFFf3e7bb},
                    {"sienna", 0xFF882d17},
                    {"silk", 0xFFbdb1a8},
                    {"silver", 0xFFc0c0c0},
                    {"sinbad", 0xFF9fd7d3},
                    {"sinopia", 0xFFcb410b},
                    {"siren", 0xFF7a013a},
                    {"sirocco", 0xFF718080},
                    {"sisal", 0xFFd3cbba},
                    {"skeptic", 0xFFcae6da},
                    {"skobeloff", 0xFF007474},
                    {"smalt", 0xFF003399},
                    {"smitten", 0xFFc84186},
                    {"smoke", 0xFF738276},
                    {"smoky", 0xFF605b73},
                    {"snow", 0xFFfffafa},
                    {"snuff", 0xFFe2d8ed},
                    {"soap", 0xFFcec8ef},
                    {"soapstone", 0xFFfffbf9},
                    {"solitaire", 0xFFfef8e2},
                    {"solitude", 0xFFeaf6ff},
                    {"sorbus", 0xFFfd7c07},
                    {"spectra", 0xFF2f5a57},
                    {"spice", 0xFF6a442e},
                    {"spindle", 0xFFb6d1ea},
                    {"spray", 0xFF79deec},
                    {"sprout", 0xFFc1d7b0},
                    {"squirrel", 0xFF8f8176},
                    {"stack", 0xFF8a8f8a},
                    {"starship", 0xFFecf245},
                    {"stiletto", 0xFF9c3336},
                    {"stonewall", 0xFF928573},
                    {"stormcloud", 0xFF4f666a},
                    {"stratos", 0xFF000741},
                    {"straw", 0xFFe4d96f},
                    {"strawberry", 0xFFfc5a8d},
                    {"strikemaster", 0xFF956387},
                    {"stromboli", 0xFF325d52},
                    {"studio", 0xFF714ab2},
                    {"submarine", 0xFFbac7c9},
                    {"sulu", 0xFFc1f07c},
                    {"sun", 0xFFfbac13},
                    {"sundance", 0xFFc9b35b},
                    {"sundown", 0xFFffb1b3},
                    {"sunflower", 0xFFe4d422},
                    {"sunglo", 0xFFe16865},
                    {"sunglow", 0xFFffcc33},
                    {"sunny", 0xFFf2f27a},
                    {"sunray", 0xFFe3ab57},
                    {"sunset", 0xFFfad6a5},
                    {"sunshade", 0xFFff9e2c},
                    {"supernova", 0xFFffc901},
                    {"surf", 0xFFbbd7c1},
                    {"sushi", 0xFF87ab39},
                    {"swamp", 0xFF001b1c},
                    {"swirl", 0xFFd3cdc5},
                    {"sycamore", 0xFF908d39},
                    {"tabasco", 0xFFa02712},
                    {"tacao", 0xFFedb381},
                    {"tacha", 0xFFd6c562},
                    {"tallow", 0xFFa8a589},
                    {"tamarillo", 0xFF991613},
                    {"tamarind", 0xFF341515},
                    {"tan", 0xFFd2b48c},
                    {"tana", 0xFFd9dcc1},
                    {"tangaroa", 0xFF03163c},
                    {"tangelo", 0xFFf94d00},
                    {"tangerine", 0xFFf28500},
                    {"tango", 0xFFed7a1c},
                    {"tapa", 0xFF7b7874},
                    {"tapestry", 0xFFb05e81},
                    {"tara", 0xFFe1f6e8},
                    {"tarawera", 0xFF073a50},
                    {"tasman", 0xFFcfdccf},
                    {"taupe", 0xFF483c32},
                    {"tea", 0xFFc1bab0},
                    {"teak", 0xFFb19461},
                    {"teal", 0xFF008080},
                    {"telemagenta", 0xFFcf3476},
                    {"temptress", 0xFF3b000b},
                    {"tenne", 0xFFcd5700},
                    {"tequila", 0xFFffe6c7},
                    {"texas", 0xFFf8f99c},
                    {"thatch", 0xFFb69d98},
                    {"thistle", 0xFFd8bfd8},
                    {"thunder", 0xFF33292f},
                    {"thunderbird", 0xFFc02b18},
                    {"tiara", 0xFFc3d1d1},
                    {"tiber", 0xFF063537},
                    {"tidal", 0xFFf1ffad},
                    {"tide", 0xFFbfb8b0},
                    {"timberwolf", 0xFFdbd7d2},
                    {"toast", 0xFF9a6e61},
                    {"toledo", 0xFF3a0020},
                    {"tolopea", 0xFF1b0245},
                    {"tomato", 0xFFff6347},
                    {"toolbox", 0xFF746cc0},
                    {"topaz", 0xFFffc87c},
                    {"tosca", 0xFF8d3f3f},
                    {"tradewind", 0xFF5fb3ac},
                    {"tranquil", 0xFFe6ffff},
                    {"travertine", 0xFFfffde8},
                    {"treehouse", 0xFF3b2820},
                    {"trinidad", 0xFFe64e03},
                    {"trout", 0xFF4a4e5a},
                    {"tuatara", 0xFF363534},
                    {"tulip", 0xFFff878d},
                    {"tumbleweed", 0xFFdeaa88},
                    {"tuna", 0xFF353542},
                    {"tundora", 0xFF4a4244},
                    {"turbo", 0xFFfae600},
                    {"turmeric", 0xFFcabb48},
                    {"turquoise", 0xFF40e0d0},
                    {"tuscany", 0xFFc09999},
                    {"tusk", 0xFFeef3c3},
                    {"tussock", 0xFFc5994b},
                    {"tutu", 0xFFfff1f9},
                    {"twilight", 0xFFe4cfde},
                    {"twine", 0xFFc2955d},
                    {"ube", 0xFF8878c3},
                    {"ultramarine", 0xFF3f00ff},
                    {"umber", 0xFF635147},
                    {"urobilin", 0xFFe1ad21},
                    {"valencia", 0xFFd84437},
                    {"valentino", 0xFF350e42},
                    {"valhalla", 0xFF2b194f},
                    {"vanilla", 0xFFf3e5ab},
                    {"varden", 0xFFfff6df},
                    {"venus", 0xFF928590},
                    {"verdigris", 0xFF43b3ae},
                    {"vermilion", 0xFFd9381e},
                    {"veronica", 0xFFa020f0},
                    {"vesuvius", 0xFFb14a0b},
                    {"victoria", 0xFF534491},
                    {"viking", 0xFF64ccdb},
                    {"viola", 0xFFcb8fa9},
                    {"violet", 0xFF7f00ff},
                    {"viridian", 0xFF40826d},
                    {"volt", 0xFFceff00},
                    {"voodoo", 0xFF533455},
                    {"vulcan", 0xFF10121d},
                    {"wafer", 0xFFdecbc6},
                    {"waiouru", 0xFF363c0d},
                    {"walnut", 0xFF773f1a},
                    {"wasabi", 0xFF788a25},
                    {"watercourse", 0xFF056f57},
                    {"waterspout", 0xFFa4f4f9},
                    {"wattle", 0xFFdcd747},
                    {"watusi", 0xFFffddcf},
                    {"wedgewood", 0xFF4e7f9e},
                    {"wenge", 0xFF645452},
                    {"westar", 0xFFdcd9d2},
                    {"wewak", 0xFFf19bab},
                    {"wheat", 0xFFf5deb3},
                    {"wheatfield", 0xFFf3edcf},
                    {"whiskey", 0xFFd59a6f},
                    {"whisper", 0xFFf7f5fa},
                    {"white", 0xFFffffff},
                    {"william", 0xFF3a686c},
                    {"windsor", 0xFF3c0878},
                    {"wine", 0xFF722f37},
                    {"wisteria", 0xFFc9a0dc},
                    {"wistful", 0xFFa4a6d3},
                    {"woodland", 0xFF4d5328},
                    {"woodrush", 0xFF302a0f},
                    {"woodsmoke", 0xFF0c0d0f},
                    {"xanadu", 0xFF738678},
                    {"yellow", 0xFFffff00},
                    {"yuma", 0xFFcec291},
                    {"zaffre", 0xFF0014a8},
                    {"zambezi", 0xFF685558},
                    {"zanah", 0xFFdaecd6},
                    {"zest", 0xFFe5841b},
                    {"zeus", 0xFF292319},
                    {"ziggurat", 0xFFbfdbe2},
                    {"zinnwaldite", 0xFFebc2af},
                    {"zircon", 0xFFf4f8ff},
                    {"zombie", 0xFFe4d69b},
                    {"zomp", 0xFF39a78e},
                    {"zorba", 0xFFa59b91},
                    {"zuccini", 0xFF044022},
                    {"zumthor", 0xFFedf6ff},
                    {"c_green", 0xFF008000},
                    {"c_red", 0xFFa31515},
                    {"c_gray", 0xFF808080},
                };
            }
        }

        public static uint ColorFromName(string name) {
            if (_colors.TryGetValue(name, out var color)) {
                return color;
            }

            return 0;
        }
    }

    public class DefaultConfiguration : XmlConfiguration {
        public DefaultConfiguration() {
            this.XmlDocument = XDocument.Parse(@"<?xml version=""1.0"" encoding=""utf-8""?>
<definitions>
  <definition name=""ASPX"" caseSensitive=""false"">
    <default>
      <font name=""Menlo"" size=""11"" style=""normal"" foreColor=""black"" backColor=""transparent""/>
    </default>
    <pattern name=""ServerSideBlock"" type=""block"" beginsWith=""&amp;lt;%"" endsWith=""%&amp;gt;"">
      <font name=""Menlo"" size=""11"" style=""normal"" foreColor=""black"" backColor=""yellow""/>
    </pattern>
    <pattern name=""Markup"" type=""markup"" highlightAttributes=""true"">
      <font name=""Menlo"" size=""11"" style=""normal"" foreColor=""maroon"" backColor=""transparent"">
        <bracketStyle foreColor=""blue"" backColor=""transparent""/>
        <attributeNameStyle foreColor=""red"" backColor=""transparent""/>
        <attributeValueStyle foreColor=""blue"" backColor=""transparent""/>
      </font>
    </pattern>
  </definition>
  <definition name=""C"" caseSensitive=""true"">
    <default>
      <font name=""Menlo"" size=""11"" style=""normal"" foreColor=""black"" backColor=""transparent""/>
    </default>
    <pattern name=""Comment"" type=""block"" beginsWith=""//"" endsWith=""\n"">
      <font name=""Menlo"" size=""11"" style=""normal"" foreColor=""green"" backColor=""transparent""/>
    </pattern>
    <pattern name=""MultiLineComment"" type=""block"" beginsWith=""/*"" endsWith=""*/"">
      <font name=""Menlo"" size=""11"" style=""normal"" foreColor=""green"" backColor=""transparent""/>
    </pattern>
    <pattern name=""String"" type=""block"" beginsWith=""&quot;"" endsWith=""&quot;"" escapesWith=""\"">
      <font name=""Menlo"" size=""11"" style=""normal"" foreColor=""darkred"" backColor=""transparent""/>
    </pattern>
    <pattern name=""WordGroup01"" type=""word"">
      <font name=""Menlo"" size=""11"" style=""normal"" foreColor=""blue"" backColor=""transparent""/>
      <word>write</word>
      <word>while</word>
      <word>volatile</word>
      <word>void</word>
      <word>unsigned</word>
      <word>union</word>
      <word>typedef</word>
      <word>time</word>
      <word>tanh</word>
      <word>tan</word>
      <word>system</word>
      <word>switch</word>
      <word>struct</word>
      <word>strcpy</word>
      <word>strcmp</word>
      <word>static</word>
      <word>srand</word>
      <word>sqrt</word>
      <word>sizeof</word>
      <word>sinh</word>
      <word>sin</word>
      <word>signed</word>
      <word>signal</word>
      <word>short</word>
      <word>scanf</word>
      <word>return</word>
      <word>rename</word>
      <word>remove</word>
      <word>register</word>
      <word>read</word>
      <word>rand</word>
      <word>putchar</word>
      <word>printf</word>
      <word>pow</word>
      <word>open</word>
      <word>malloc</word>
      <word>long</word>
      <word>log10</word>
      <word>log</word>
      <word>labs</word>
      <word>int</word>
      <word>if</word>
      <word>goto</word>
      <word>getenv</word>
      <word>getchar</word>
      <word>free</word>
      <word>for</word>
      <word>floor</word>
      <word>float</word>
      <word>fabs</word>
      <word>extern</word>
      <word>exp</word>
      <word>exit</word>
      <word>enum</word>
      <word>else</word>
      <word>double</word>
      <word>do</word>
      <word>div</word>
      <word>default</word>
      <word>cosh</word>
      <word>cos</word>
      <word>continue</word>
      <word>const</word>
      <word>close</word>
      <word>clock</word>
      <word>char</word>
      <word>ceil</word>
      <word>case</word>
      <word>calloc</word>
      <word>break</word>
      <word>auto</word>
      <word>atol</word>
      <word>atoi</word>
      <word>atof</word>
      <word>atan</word>
      <word>asin</word>
      <word>acos</word>
      <word>abs</word>
      <word>abort</word>
    </pattern>
  </definition>
  <definition name=""C++"" caseSensitive=""true"">
    <default>
      <font name=""Menlo"" size=""11"" style=""normal"" foreColor=""black"" backColor=""transparent""/>
    </default>
    <pattern name=""Comment"" type=""block"" beginsWith=""//"" endsWith=""\n"">
      <font name=""Menlo"" size=""11"" style=""normal"" foreColor=""green"" backColor=""transparent""/>
    </pattern>
    <pattern name=""MultiLineComment"" type=""block"" beginsWith=""/*"" endsWith=""*/"">
      <font name=""Menlo"" size=""11"" style=""normal"" foreColor=""green"" backColor=""transparent""/>
    </pattern>
    <pattern name=""String"" type=""block"" beginsWith=""&quot;"" endsWith=""&quot;"" escapesWith=""\"">
      <font name=""Menlo"" size=""11"" style=""normal"" foreColor=""darkred"" backColor=""transparent""/>
    </pattern>
    <pattern name=""WordGroup01"" type=""word"">
      <font name=""Menlo"" size=""11"" style=""normal"" foreColor=""blue"" backColor=""transparent""/>
      <word>xor</word>
      <word>while</word>
      <word>volatile</word>
      <word>void</word>
      <word>virtual</word>
      <word>using</word>
      <word>unsigned</word>
      <word>union</word>
      <word>u8</word>
      <word>u64</word>
      <word>u32</word>
      <word>u16</word>
      <word>u128</word>
      <word>typename</word>
      <word>typedef</word>
      <word>try</word>
      <word>true</word>
      <word>throw</word>
      <word>this</word>
      <word>template</word>
      <word>switch</word>
      <word>struct</word>
      <word>string</word>
      <word>static_cast</word>
      <word>static</word>
      <word>sizeof</word>
      <word>signed</word>
      <word>short</word>
      <word>s8</word>
      <word>s64</word>
      <word>s32</word>
      <word>s16</word>
      <word>s128</word>
      <word>return</word>
      <word>reinterpret_cast</word>
      <word>register</word>
      <word>public</word>
      <word>protected</word>
      <word>private</word>
      <word>or</word>
      <word>operator</word>
      <word>not</word>
      <word>new</word>
      <word>near</word>
      <word>namespace</word>
      <word>mutable</word>
      <word>long</word>
      <word>int</word>
      <word>inline</word>
      <word>if</word>
      <word>huge</word>
      <word>goto</word>
      <word>friend</word>
      <word>for</word>
      <word>float</word>
      <word>fixed</word>
      <word>finally</word>
      <word>far</word>
      <word>false</word>
      <word>f32</word>
      <word>extern</word>
      <word>explicit</word>
      <word>except</word>
      <word>enum</word>
      <word>else</word>
      <word>dynamic_cast</word>
      <word>double</word>
      <word>do</word>
      <word>delete</word>
      <word>default</word>
      <word>continue</word>
      <word>const_cast</word>
      <word>const</word>
      <word>class</word>
      <word>char</word>
      <word>catch</word>
      <word>case</word>
      <word>break</word>
      <word>bool</word>
      <word>auto</word>
      <word>asm</word>
      <word>and</word>
      <word>#warning</word>
      <word>#undef</word>
      <word>#pragma</word>
      <word>#line</word>
      <word>#include</word>
      <word>#ifndef</word>
      <word>#ifdef</word>
      <word>#if</word>
      <word>#error</word>
      <word>#endif</word>
      <word>#else</word>
      <word>#elif</word>
      <word>#define</word>
    </pattern>
  </definition>
  <definition name=""C#"" caseSensitive=""true"">
    <default>
      <font name=""Menlo"" size=""14"" style=""normal"" foreColor=""black"" backColor=""transparent""/>
    </default>
    <pattern name=""Keyword"" type=""word"">
      <font name=""Menlo"" size=""14"" style=""normal"" foreColor=""blue"" backColor=""transparent""/>
      <word>as</word>
      <word>is</word>
      <word>new</word>
      <word>sizeof</word>
      <word>typeof</word>
      <word>true</word>
      <word>false</word>
      <word>stackalloc</word>
      <word>explicit</word>
      <word>implicit</word>
      <word>operator</word>
      <word>base</word>
      <word>this</word>
      <word>null</word>
    </pattern>
    <pattern name=""Namespace"" type=""word"">
      <font name=""Menlo"" size=""14"" style=""normal"" foreColor=""blue"" backColor=""transparent""/>
      <word>namespace</word>
      <word>using</word>
    </pattern>
    <pattern name=""MethodParameter"" type=""word"">
      <font name=""Menlo"" size=""14"" style=""normal"" foreColor=""blue"" backColor=""transparent""/>
      <word>params</word>
      <word>ref</word>
      <word>out</word>
    </pattern>
    <pattern name=""Statement"" type=""word"">
      <font name=""Menlo"" size=""14"" style=""normal"" foreColor=""blue"" backColor=""transparent""/>
      <word>if</word>
      <word>else</word>
      <word>switch</word>
      <word>case</word>
      <word>do</word>
      <word>for</word>
      <word>foreach</word>
      <word>in</word>
      <word>while</word>
      <word>break</word>
      <word>continue</word>
      <word>goto</word>
      <word>return</word>
      <word>try</word>
      <word>throw</word>
      <word>catch</word>
      <word>finally</word>
      <word>checked</word>
      <word>unchecked</word>
      <word>fixed</word>
      <word>lock</word>
    </pattern>
    <pattern name=""Modifier"" type=""word"">
      <font name=""Menlo"" size=""14"" style=""normal"" foreColor=""blue"" backColor=""transparent""/>
      <word>internal</word>
      <word>private</word>
      <word>protected</word>
      <word>public</word>
      <word>abstract</word>
      <word>const</word>
      <word>default</word>
      <word>event</word>
      <word>extern</word>
      <word>override</word>
      <word>readonly</word>
      <word>sealed</word>
      <word>static</word>
      <word>unsafe</word>
      <word>virtual</word>
      <word>volatile</word>
    </pattern>
    <pattern name=""ValueType"" type=""word"">
      <font name=""Menlo"" size=""14"" style=""normal"" foreColor=""blue"" backColor=""transparent""/>
      <word>void</word>
      <word>bool</word>
      <word>byte</word>
      <word>char</word>
      <word>decimal</word>
      <word>double</word>
      <word>enum</word>
      <word>float</word>
      <word>int</word>
      <word>long</word>
      <word>sbyte</word>
      <word>short</word>
      <word>struct</word>
      <word>uint</word>
      <word>ulong</word>
      <word>ushort</word>
    </pattern>
    <pattern name=""ReferenceType"" type=""word"">
      <font name=""Menlo"" size=""14"" style=""normal"" foreColor=""blue"" backColor=""transparent""/>
      <word>class</word>
      <word>interface</word>
      <word>delegate</word>
      <word>object</word>
      <word>string</word>
    </pattern>
    <pattern name=""Operator"" type=""word"">
      <font name=""Menlo"" size=""14"" style=""normal"" foreColor=""purple"" backColor=""transparent""/>
      <word>+</word>
      <word>-</word>
      <word>=</word>
      <word>%</word>
      <word>*</word>
      <word>/</word>
      <word>&amp;</word>
      <word>|</word>
    </pattern>
    <pattern name=""String"" type=""block"" beginsWith=""&amp;quot;"" endsWith=""&amp;quot;"" escapesWith=""\"">
      <font name=""Menlo"" size=""14"" style=""normal"" foreColor=""c_red"" backColor=""transparent""/>
    </pattern>
    <pattern name=""VerbatimString"" type=""block"" beginsWith=""@&amp;quot;"" endsWith=""&amp;quot;"">
      <font name=""Menlo"" size=""14"" style=""normal"" foreColor=""c_red"" backColor=""transparent""/>
    </pattern>
    <pattern name=""Comment"" type=""block"" beginsWith=""//"" endsWith=""\n"">
      <font name=""Menlo"" size=""14"" style=""normal"" foreColor=""c_green"" backColor=""transparent""/>
    </pattern>
    <pattern name=""MultiLineComment"" type=""block"" beginsWith=""/*"" endsWith=""*/"">
      <font name=""Menlo"" size=""14"" style=""normal"" foreColor=""c_gray"" backColor=""transparent""/>
    </pattern>
    <pattern name=""PreprocessorDirective"" type=""word"">
      <font name=""Menlo"" size=""14"" style=""normal"" foreColor=""maroon"" backColor=""transparent""/>
      <word>#if</word>
      <word>#else</word>
      <word>#elif</word>
      <word>#endif</word>
      <word>#define</word>
      <word>#undef</word>
      <word>#warning</word>
      <word>#error</word>
      <word>#line</word>
      <word>#region</word>
      <word>#endregion</word>
      <word>#pragma</word>
      <word>#checksum</word>
    </pattern>
  </definition>
  <definition name=""COBOL"" caseSensitive=""true"">
    <default>
      <font name=""Menlo"" size=""11"" style=""normal"" foreColor=""black"" backColor=""transparent""/>
    </default>
    <pattern name=""Comment"" type=""block"" beginsWith=""//"" endsWith=""\n"">
      <font name=""Menlo"" size=""11"" style=""normal"" foreColor=""green"" backColor=""transparent""/>
    </pattern>
    <pattern name=""MultiLineComment"" type=""block"" beginsWith=""/*"" endsWith=""*/"">
      <font name=""Menlo"" size=""11"" style=""normal"" foreColor=""green"" backColor=""transparent""/>
    </pattern>
    <pattern name=""String"" type=""block"" beginsWith=""&quot;"" endsWith=""&quot;"" escapesWith=""\"">
      <font name=""Menlo"" size=""11"" style=""normal"" foreColor=""darkred"" backColor=""transparent""/>
    </pattern>
    <pattern name=""WordGroup01"" type=""word"">
      <font name=""Menlo"" size=""11"" style=""normal"" foreColor=""blue"" backColor=""transparent""/>
      <word>ZEROS</word>
      <word>ZERO-FILL</word>
      <word>ZEROES</word>
      <word>ZER</word>
      <word>WRITE-ONLY</word>
      <word>WRITE</word>
      <word>WORKING-STORAGE</word>
      <word>WORDS</word>
      <word>WITHIN</word>
      <word>WITH</word>
      <word>WHEN-COMPILED</word>
      <word>WHEN</word>
      <word>WAIT</word>
      <word>VARYING</word>
      <word>VALUES</word>
      <word>VALUE</word>
      <word>VALIDATE</word>
      <word>VALID</word>
      <word>USING</word>
      <word>USE</word>
      <word>USAGE-MODE</word>
      <word>USAGE</word>
      <word>UPON</word>
      <word>UPDATE</word>
      <word>UP</word>
      <word>UNTIL</word>
      <word>UNSTRING</word>
      <word>UNLOCK</word>
      <word>UNIVERSAL</word>
      <word>UNIT</word>
      <word>UNEQUAL</word>
      <word>UNDERLINE</word>
      <word>TYPEDEF</word>
      <word>TYPE</word>
      <word>TRUE</word>
      <word>TRANSCEIVE</word>
      <word>TRANSACTION</word>
      <word>TRAILING-SIGN</word>
      <word>TRAILING</word>
      <word>TRACKS</word>
      <word>TRACK-OVERFLOW</word>
      <word>TRACK-LIMIT</word>
      <word>TRACK-AREA</word>
      <word>TRACK</word>
      <word>TRACE</word>
      <word>TOP</word>
      <word>TO</word>
      <word>TITLE</word>
      <word>TIMES</word>
      <word>TIMEOUT</word>
      <word>TIME</word>
      <word>THRU</word>
      <word>THROUGH</word>
      <word>THEN</word>
      <word>THAN</word>
      <word>TEXT</word>
      <word>TEST</word>
      <word>TERMINATE</word>
      <word>TERMINAL</word>
      <word>TENNANT</word>
      <word>TENANT</word>
      <word>TAPE</word>
      <word>TALLYING</word>
      <word>TALLY</word>
      <word>TABLE</word>
      <word>SYNCHRONIZED</word>
      <word>SYNC</word>
      <word>SYMBOLIC</word>
      <word>SUPPRESS</word>
      <word>SUPER</word>
      <word>SUM</word>
      <word>SUFFIX</word>
      <word>SUCCESSIVE</word>
      <word>SUBTRACT</word>
      <word>SUBSCHEMA-NAME</word>
      <word>SUB-SCHEMA</word>
      <word>SUBRANGE</word>
      <word>SUB-QUEUE-3</word>
      <word>SUB-QUEUE-2</word>
      <word>SUB-QUEUE-1</word>
      <word>STRING</word>
      <word>STORE</word>
      <word>STOP</word>
      <word>STATUS</word>
      <word>STATIONS</word>
      <word>STATION</word>
      <word>START</word>
      <word>STANDARD-2</word>
      <word>STANDARD-1</word>
      <word>STANDARD</word>
      <word>SPECIAL-NAMES</word>
      <word>SPACES</word>
      <word>SPACE-FILL</word>
      <word>SPACE</word>
      <word>SOURCE-COMPUTER</word>
      <word>SOURCE</word>
      <word>SORT-STATUS</word>
      <word>SORT-RETURN</word>
      <word>SORT-MODE-SIZE</word>
      <word>SORT-MESSAGE</word>
      <word>SORT-MERGE</word>
      <word>SORT-FILE-SIZE</word>
      <word>SORT-CORE-SIZE</word>
      <word>SORT-CONTROL</word>
      <word>SORT</word>
      <word>SKIP3</word>
      <word>SKIP2</word>
      <word>SKIP1</word>
      <word>SIZE</word>
      <word>SINGLE</word>
      <word>SIMPLE</word>
      <word>SIGN</word>
      <word>SHIFT-OUT</word>
      <word>SHIFT-IN</word>
      <word>SHARED</word>
      <word>SET</word>
      <word>SESSION-ID</word>
      <word>SESSION</word>
      <word>SERVICE</word>
      <word>SEQUENTIAL</word>
      <word>SEQUENCE</word>
      <word>SEPARATE</word>
      <word>SENTENCE</word>
      <word>SEND</word>
      <word>SELF</word>
      <word>SELECTIVE</word>
      <word>SELECTED</word>
      <word>SELECT</word>
      <word>SEGMENT-LIMIT</word>
      <word>SEGMENT</word>
      <word>SEEK</word>
      <word>SECURITY</word>
      <word>SECURE</word>
      <word>SECTION</word>
      <word>SEARCH</word>
      <word>SD</word>
      <word>SCREEN</word>
      <word>SAVED-AREA</word>
      <word>SAME</word>
      <word>SA</word>
      <word>RUN</word>
      <word>ROUNDED</word>
      <word>ROLL-OUT</word>
      <word>ROLLBACK</word>
      <word>RIGHT-JUSTIFY</word>
      <word>RIGHT</word>
      <word>RH</word>
      <word>RF</word>
      <word>REWRITE</word>
      <word>REWIND</word>
      <word>REVERSE-VIDEO</word>
      <word>REVERSED</word>
      <word>RETURNING</word>
      <word>RETURN-CODE</word>
      <word>RETURN</word>
      <word>RETRIEVAL</word>
      <word>RETAINING</word>
      <word>RESET</word>
      <word>RESERVE</word>
      <word>RERUN</word>
      <word>REQUIRED</word>
      <word>REPOSITORY</word>
      <word>REPORTS</word>
      <word>REPORTING</word>
      <word>REPORT</word>
      <word>REPLACING</word>
      <word>REPLACE</word>
      <word>REPEATED</word>
      <word>REORG-CRITERIA</word>
      <word>RENAMES</word>
      <word>REMOVAL</word>
      <word>REMARKS</word>
      <word>REMAINDER</word>
      <word>RELOAD</word>
      <word>RELEASE</word>
      <word>RELATIVE</word>
      <word>RELATION</word>
      <word>REFERENCES</word>
      <word>REFERENCE</word>
      <word>REEL</word>
      <word>REDEFINES</word>
      <word>RECORDS</word>
      <word>RECORD-OVERFLOW</word>
      <word>RECORD-NAME</word>
      <word>RECORDING</word>
      <word>RECORD</word>
      <word>RECONNECT</word>
      <word>RECEIVE</word>
      <word>REALM</word>
      <word>READY</word>
      <word>READ</word>
      <word>RD</word>
      <word>RANGE</word>
      <word>RANDOM</word>
      <word>RAISING</word>
      <word>RAISE</word>
      <word>QUOTES</word>
      <word>QUOTE</word>
      <word>QUEUE</word>
      <word>PURGE</word>
      <word>PROTOPTYPE</word>
      <word>PROTECTED</word>
      <word>PROPERTY</word>
      <word>PROMPT</word>
      <word>PROGRAM-STATUS</word>
      <word>PROGRAM-ID</word>
      <word>PROGRAM</word>
      <word>PROCESSING</word>
      <word>PROCEED</word>
      <word>PROCEDURES</word>
      <word>PROCEDURE</word>
      <word>PRIOR</word>
      <word>PRINTING</word>
      <word>PREVIOUS</word>
      <word>PRESENT</word>
      <word>PREFIX</word>
      <word>POSITIVE</word>
      <word>POSITIONING</word>
      <word>POSITION</word>
      <word>POINTER</word>
      <word>PLUS</word>
      <word>PICTURE</word>
      <word>PIC</word>
      <word>PH</word>
      <word>PF</word>
      <word>PERFORM</word>
      <word>PASSWORD</word>
      <word>PARAGRAPH</word>
      <word>PAGE-COUNTER</word>
      <word>PAGE</word>
      <word>PADDING</word>
      <word>PACKED-DECIMAL</word>
      <word>OWNER</word>
      <word>OVERRIDE</word>
      <word>OVERLINE</word>
      <word>OVERFLOW</word>
      <word>OUTPUT</word>
      <word>OTHERWISE</word>
      <word>OTHER</word>
      <word>ORGANIZATION</word>
      <word>ORDER</word>
      <word>OR</word>
      <word>OPTIONAL</word>
      <word>OPEN</word>
      <word>ONLY</word>
      <word>ON</word>
      <word>OMITTED</word>
      <word>OFF</word>
      <word>OF</word>
      <word>OCCURS</word>
      <word>OBJECT-COMPUTER</word>
      <word>OBJECT</word>
      <word>NUMERIC-EDITED</word>
      <word>NUMERIC</word>
      <word>NUMBER</word>
      <word>NULLS</word>
      <word>NULL</word>
      <word>NOTE</word>
      <word>NOT</word>
      <word>NONE</word>
      <word>NOMINAL</word>
      <word>NO</word>
      <word>NEXT</word>
      <word>NEGATIVE</word>
      <word>NATIVE</word>
      <word>NATIONAL-EDITED</word>
      <word>NATIONAL</word>
      <word>NAMED</word>
      <word>MULTIPLY</word>
      <word>MULTIPLE</word>
      <word>MULTICONVERSATIONMODE</word>
      <word>MULTICON</word>
      <word>MOVE</word>
      <word>MORE-LABELS</word>
      <word>MODULES</word>
      <word>MODIFY</word>
      <word>MODE-3</word>
      <word>MODE-2</word>
      <word>MODE-1</word>
      <word>MODE</word>
      <word>METHOD-ID</word>
      <word>METHOD</word>
      <word>MESSAGE</word>
      <word>MERGE</word>
      <word>MEMORY</word>
      <word>MEMBER</word>
      <word>MANUAL</word>
      <word>LOW-VALUES</word>
      <word>LOW-VALUE</word>
      <word>LOWLIGHT</word>
      <word>LOCK</word>
      <word>LOCALLY</word>
      <word>LINKAGE</word>
      <word>LINES</word>
      <word>LINE-COUNTER</word>
      <word>LINE</word>
      <word>LINAGE-COUNTER</word>
      <word>LINAGE</word>
      <word>LIMITS</word>
      <word>LIMITED</word>
      <word>LIMIT</word>
      <word>LESS</word>
      <word>LENGTH</word>
      <word>LEFTLINE</word>
      <word>LEFT-JUSTIFY</word>
      <word>LEFT</word>
      <word>LEADING</word>
      <word>LD</word>
      <word>LAST</word>
      <word>LABEL</word>
      <word>KEY</word>
      <word>KEEP</word>
      <word>KANJI</word>
      <word>JUSTIFIED</word>
      <word>JUST</word>
      <word>JOINING</word>
      <word>JOB</word>
      <word>JAPANESE</word>
      <word>IS</word>
      <word>I-O-CONTROL</word>
      <word>INVOKE</word>
      <word>INVARIANT</word>
      <word>INVALID</word>
      <word>INT</word>
      <word>INSTALLATION</word>
      <word>INSPECT</word>
      <word>INPUT-OUTPUT</word>
      <word>INPUT</word>
      <word>INITIATE</word>
      <word>INITIALIZE</word>
      <word>INITIAL</word>
      <word>INHERITS</word>
      <word>INDICATE</word>
      <word>INDEX-n</word>
      <word>INDEXED</word>
      <word>INDEX</word>
      <word>INCLUDE</word>
      <word>IN</word>
      <word>IF</word>
      <word>IDENTIFICATION</word>
      <word>ID</word>
      <word>I-</word>
      <word>HIGH-VALUES</word>
      <word>HIGH-VALUE</word>
      <word>HIGHLIGHT</word>
      <word>HEADING</word>
      <word>GROUP</word>
      <word>GRID</word>
      <word>GREATER</word>
      <word>GOBACK</word>
      <word>GLOBAL</word>
      <word>GIVING</word>
      <word>GET</word>
      <word>GENERATE</word>
      <word>G</word>
      <word>FUNCTION</word>
      <word>FULL</word>
      <word>FROM</word>
      <word>FREE</word>
      <word>FORMATTED</word>
      <word>FORMAT</word>
      <word>FORM</word>
      <word>FOREGROUND-COLOR</word>
      <word>FOR</word>
      <word>FOOTING</word>
      <word>FLADD</word>
      <word>FIRST</word>
      <word>FINISH</word>
      <word>FIND</word>
      <word>FINAL</word>
      <word>FILLER</word>
      <word>FILES</word>
      <word>FILE-LIMITS</word>
      <word>FILE-LIMIT</word>
      <word>FILE-CONTROL</word>
      <word>FILE</word>
      <word>FETCH</word>
      <word>FD</word>
      <word>FALSE</word>
      <word>FACTORY</word>
      <word>EXTERNAL</word>
      <word>EXTEND</word>
      <word>EXOR</word>
      <word>EXIT</word>
      <word>EXEC</word>
      <word>EXCLUSIVE</word>
      <word>EXCEPTION-OBJECT</word>
      <word>EXCEPTION</word>
      <word>EXCEEDS</word>
      <word>EXAMINE</word>
      <word>EXACT</word>
      <word>EVERY</word>
      <word>EVALUATE</word>
      <word>ESI</word>
      <word>ERROR</word>
      <word>ERASE</word>
      <word>EQUALS</word>
      <word>EQUAL</word>
      <word>EOS</word>
      <word>EOP</word>
      <word>EOL</word>
      <word>ENVIRONMENT</word>
      <word>ENTRY</word>
      <word>ENTER</word>
      <word>END-WRITE</word>
      <word>END-UNSTRING</word>
      <word>END-TRANSCEIVE</word>
      <word>END-SUBTRACT</word>
      <word>END-STRING</word>
      <word>END-START</word>
      <word>END-SEND</word>
      <word>END-SEARCH</word>
      <word>END-REWRITE</word>
      <word>END-RETURN</word>
      <word>END-RECEIVE</word>
      <word>END-READ</word>
      <word>END-PERFORM</word>
      <word>END-OF-PAGE</word>
      <word>END-MULTIPLY</word>
      <word>END-INVOKE</word>
      <word>ENDING</word>
      <word>END-IF</word>
      <word>END-EXEC</word>
      <word>END-EVALUATE</word>
      <word>END-ENABLE</word>
      <word>END-DIVIDE</word>
      <word>END-DISPLAY</word>
      <word>END-DISABLE</word>
      <word>END-DELETE</word>
      <word>END-COMPUTE</word>
      <word>ENDCOBOL</word>
      <word>END-CALL</word>
      <word>END-ADD</word>
      <word>END-ACCEPT</word>
      <word>END</word>
      <word>ENABLE</word>
      <word>EMPTY</word>
      <word>EMI</word>
      <word>ELSE</word>
      <word>EJECT</word>
      <word>EGI</word>
      <word>EGCS</word>
      <word>EDIT-STATUS</word>
      <word>EDIT-OPTION</word>
      <word>EDIT-MODE</word>
      <word>EDIT-CURSOR</word>
      <word>EDIT-COLOR</word>
      <word>DYNAMIC</word>
      <word>DUPLICATES</word>
      <word>DUPLICATE</word>
      <word>DOWN</word>
      <word>DIVISION</word>
      <word>DIVIDE</word>
      <word>DISPLAY-n</word>
      <word>DISPLAY-EXIT</word>
      <word>DISPLAY-1</word>
      <word>DISPLAY</word>
      <word>DISJOINING</word>
      <word>DISCONNECT</word>
      <word>DISABLE</word>
      <word>DIRECT</word>
      <word>DEVICE</word>
      <word>DETAIL</word>
      <word>DESTINATION-3</word>
      <word>DESTINATION-2</word>
      <word>DESTINATION-1</word>
      <word>DESTINATION</word>
      <word>DESCENDING</word>
      <word>DEPENDING</word>
      <word>DELIMITER</word>
      <word>DELIMITED</word>
      <word>DELETE</word>
      <word>DEFAULT</word>
      <word>DECLARATIVES</word>
      <word>DECIMAL-POINT</word>
      <word>DEBUG-SUB-3</word>
      <word>DEBUG-SUB-2</word>
      <word>DEBUG-SUB-1</word>
      <word>DEBUG-NAME</word>
      <word>DEBUG-LINE</word>
      <word>DEBUG-ITEM</word>
      <word>DEBUGGING</word>
      <word>DEBUG-CONTENTS</word>
      <word>DEAD-LOCK</word>
      <word>DE</word>
      <word>DB-STATUS</word>
      <word>DB-SET-NAME</word>
      <word>DB-RECORD-NAME</word>
      <word>DB-EXCEPTION</word>
      <word>DB-DATA-NAME</word>
      <word>DB-ACCESS-CONTROL-KEY</word>
      <word>DB</word>
      <word>DAY-OF-WEEK</word>
      <word>DAY</word>
      <word>DATE-WRITTEN</word>
      <word>DATE-COMPILED</word>
      <word>DATE</word>
      <word>DATA</word>
      <word>CURSOR</word>
      <word>CURRENT</word>
      <word>CURRENCY</word>
      <word>CRT-UNDER</word>
      <word>CRT</word>
      <word>CRP</word>
      <word>COUNT</word>
      <word>CORRESPONDING</word>
      <word>CORR</word>
      <word>CORE-INDEX</word>
      <word>COPY</word>
      <word>CONVERTING</word>
      <word>CONTROLS</word>
      <word>CONTROL-CHARACTER</word>
      <word>CONTROL</word>
      <word>CONTINUE</word>
      <word>CONTENT</word>
      <word>CONTAINS</word>
      <word>CONTAINED</word>
      <word>CONSTANT</word>
      <word>CONNECT</word>
      <word>CONFIGURATION</word>
      <word>COM-REG</word>
      <word>COMPUTE</word>
      <word>COMPUTATIONAL-n</word>
      <word>COMPUTATIONAL-5</word>
      <word>COMPUTATIONAL-4</word>
      <word>COMPUTATIONAL-3</word>
      <word>COMPUTATIONAL-2</word>
      <word>COMPUTATIONAL-1</word>
      <word>COMPUTATIONAL</word>
      <word>COMP-n</word>
      <word>COMPLEX</word>
      <word>COMP-5</word>
      <word>COMP-4</word>
      <word>COMP-3</word>
      <word>COMP-2</word>
      <word>COMP-1</word>
      <word>COMP</word>
      <word>COMMUNICATION</word>
      <word>COMMON</word>
      <word>COMMIT</word>
      <word>COMMAND</word>
      <word>COMMA</word>
      <word>COLUMN</word>
      <word>COLLATING</word>
      <word>CODE-SET</word>
      <word>CODE</word>
      <word>COBOL</word>
      <word>CLOSE</word>
      <word>CLOCK-UNITS</word>
      <word>CLASS-ID</word>
      <word>CLASS</word>
      <word>CHARACTERS</word>
      <word>CHARACTER</word>
      <word>CHANGED</word>
      <word>CH</word>
      <word>CF</word>
      <word>CD</word>
      <word>CBL</word>
      <word>CANCEL</word>
      <word>CALL</word>
      <word>BY</word>
      <word>BOTTOM</word>
      <word>B-OR</word>
      <word>BOOLEAN</word>
      <word>B-NOT</word>
      <word>BLOCK</word>
      <word>BLINK</word>
      <word>B-LESS</word>
      <word>BLANK</word>
      <word>BITS</word>
      <word>BIT</word>
      <word>BINARY</word>
      <word>B-EXOR</word>
      <word>BELL</word>
      <word>BEGINNING</word>
      <word>BEFORE</word>
      <word>BASED-STORAGE</word>
      <word>BASED</word>
      <word>B-AND</word>
      <word>BACKGROUND-COLOR</word>
      <word>AUTOMATIC</word>
      <word>AUTO</word>
      <word>AUTHOR</word>
      <word>AT</word>
      <word>ASSIGN</word>
      <word>ASCENDING</word>
      <word>AS</word>
      <word>ARITHMETIC</word>
      <word>AREAS</word>
      <word>AREA</word>
      <word>ARE</word>
      <word>APPLY</word>
      <word>ANY</word>
      <word>AND</word>
      <word>ALTERNATE</word>
      <word>ALTER</word>
      <word>ALS</word>
      <word>ALPHANUMERIC-EDITED</word>
      <word>ALPHANUMERIC</word>
      <word>ALPHABETIC-UPPER</word>
      <word>ALPHABETIC-LOWER</word>
      <word>ALPHABETIC</word>
      <word>ALPHABET</word>
      <word>ALL</word>
      <word>AFTER</word>
      <word>ADVANCING</word>
      <word>ADDRESS</word>
      <word>ADD</word>
      <word>ACTUAL</word>
      <word>ACCESS</word>
      <word>ACCEPT</word>
    </pattern>
  </definition>
  <definition name=""Eiffel"" caseSensitive=""true"">
    <default>
      <font name=""Menlo"" size=""11"" style=""normal"" foreColor=""black"" backColor=""transparent""/>
    </default>
    <pattern name=""Comment"" type=""block"" beginsWith=""//"" endsWith=""\n"">
      <font name=""Menlo"" size=""11"" style=""normal"" foreColor=""green"" backColor=""transparent""/>
    </pattern>
    <pattern name=""MultiLineComment"" type=""block"" beginsWith=""/*"" endsWith=""*/"">
      <font name=""Menlo"" size=""11"" style=""normal"" foreColor=""green"" backColor=""transparent""/>
    </pattern>
    <pattern name=""String"" type=""block"" beginsWith=""&quot;"" endsWith=""&quot;"" escapesWith=""\"">
      <font name=""Menlo"" size=""11"" style=""normal"" foreColor=""darkred"" backColor=""transparent""/>
    </pattern>
    <pattern name=""WordGroup01"" type=""word"">
      <font name=""Menlo"" size=""11"" style=""normal"" foreColor=""blue"" backColor=""transparent""/>
      <word>xor</word>
      <word>when</word>
      <word>variant</word>
      <word>until</word>
      <word>Unique</word>
      <word>undefine</word>
      <word>True</word>
      <word>then</word>
      <word>Strip</word>
      <word>separate</word>
      <word>select</word>
      <word>retry</word>
      <word>rescue</word>
      <word>require</word>
      <word>rename</word>
      <word>redefine</word>
      <word>prefix</word>
      <word>or</word>
      <word>once</word>
      <word>old</word>
      <word>obsolete</word>
      <word>not</word>
      <word>loop</word>
      <word>local</word>
      <word>like</word>
      <word>is</word>
      <word>invariant</word>
      <word>inspect</word>
      <word>inherit</word>
      <word>infix</word>
      <word>indexing</word>
      <word>implies</word>
      <word>if</word>
      <word>frozen</word>
      <word>from</word>
      <word>feature</word>
      <word>False</word>
      <word>external</word>
      <word>export</word>
      <word>expanded</word>
      <word>ensure</word>
      <word>end</word>
      <word>elseif</word>
      <word>else</word>
      <word>do</word>
      <word>deferred</word>
      <word>debug</word>
      <word>creation</word>
      <word>class</word>
      <word>check</word>
      <word>as</word>
      <word>and</word>
      <word>all</word>
      <word>alias</word>
    </pattern>
  </definition>
  <definition name=""Fortran"" caseSensitive=""true"">
    <default>
      <font name=""Menlo"" size=""11"" style=""normal"" foreColor=""black"" backColor=""transparent""/>
    </default>
    <pattern name=""Comment"" type=""block"" beginsWith=""//"" endsWith=""\n"">
      <font name=""Menlo"" size=""11"" style=""normal"" foreColor=""green"" backColor=""transparent""/>
    </pattern>
    <pattern name=""MultiLineComment"" type=""block"" beginsWith=""/*"" endsWith=""*/"">
      <font name=""Menlo"" size=""11"" style=""normal"" foreColor=""green"" backColor=""transparent""/>
    </pattern>
    <pattern name=""String"" type=""block"" beginsWith=""&quot;"" endsWith=""&quot;"" escapesWith=""\"">
      <font name=""Menlo"" size=""11"" style=""normal"" foreColor=""darkred"" backColor=""transparent""/>
    </pattern>
    <pattern name=""WordGroup01"" type=""word"">
      <font name=""Menlo"" size=""11"" style=""normal"" foreColor=""blue"" backColor=""transparent""/>
      <word>WRITE</word>
      <word>WHILE</word>
      <word>UNION</word>
      <word>THEN</word>
      <word>SUBROUTINE</word>
      <word>STRUCTURE</word>
      <word>STOP</word>
      <word>SELECT</word>
      <word>SAVE</word>
      <word>REWIND</word>
      <word>RETURN</word>
      <word>RECORD</word>
      <word>REAL</word>
      <word>READ</word>
      <word>PROGRAM</word>
      <word>PRINT</word>
      <word>PRECISION</word>
      <word>PAUSE</word>
      <word>PARAMETER</word>
      <word>OPEN</word>
      <word>NAMELIST</word>
      <word>MAP</word>
      <word>LOGICAL</word>
      <word>LOCKING</word>
      <word>INTRINSIC</word>
      <word>INTERFACE TO</word>
      <word>INTEGER</word>
      <word>INQUIRE</word>
      <word>INCLUDE</word>
      <word>IMPLICIT</word>
      <word>IF</word>
      <word>GOTO</word>
      <word>FUNCTION</word>
      <word>FORMAT</word>
      <word>EXTERNAL</word>
      <word>EXIT</word>
      <word>EQUIVALENCE</word>
      <word>ENTRY</word>
      <word>ENDFILE</word>
      <word>END</word>
      <word>END</word>
      <word>END</word>
      <word>ELSE</word>
      <word>DOUBLE</word>
      <word>DO</word>
      <word>DO</word>
      <word>DIMENSION</word>
      <word>DEALLOCATE</word>
      <word>DATA</word>
      <word>DATA</word>
      <word>CYCLE</word>
      <word>CONTINUE</word>
      <word>COMPLEX</word>
      <word>COMPLEX</word>
      <word>COMMON</word>
      <word>CLOSE</word>
      <word>CHARACTER</word>
      <word>CASE</word>
      <word>CASE</word>
      <word>CALL</word>
      <word>BYTE</word>
      <word>BLOCK</word>
      <word>BACKSPACE</word>
      <word>AUTOMATIC</word>
      <word>ASSIGN</word>
      <word>ALLOCATE</word>
    </pattern>
  </definition>
  <definition name=""Haskell"" caseSensitive=""true"">
    <default>
      <font name=""Menlo"" size=""11"" style=""normal"" foreColor=""black"" backColor=""transparent""/>
    </default>
    <pattern name=""Comment"" type=""block"" beginsWith=""//"" endsWith=""\n"">
      <font name=""Menlo"" size=""11"" style=""normal"" foreColor=""green"" backColor=""transparent""/>
    </pattern>
    <pattern name=""MultiLineComment"" type=""block"" beginsWith=""/*"" endsWith=""*/"">
      <font name=""Menlo"" size=""11"" style=""normal"" foreColor=""green"" backColor=""transparent""/>
    </pattern>
    <pattern name=""String"" type=""block"" beginsWith=""&quot;"" endsWith=""&quot;"" escapesWith=""\"">
      <font name=""Menlo"" size=""11"" style=""normal"" foreColor=""darkred"" backColor=""transparent""/>
    </pattern>
    <pattern name=""WordGroup01"" type=""word"">
      <font name=""Menlo"" size=""11"" style=""normal"" foreColor=""blue"" backColor=""transparent""/>
      <word>zipWith3</word>
      <word>zipWith</word>
      <word>zip3</word>
      <word>zip</word>
      <word>writeFile</word>
      <word>words</word>
      <word>userError</word>
      <word>unzip3</word>
      <word>unzip</word>
      <word>unwords</word>
      <word>until</word>
      <word>unlines</word>
      <word>undefined</word>
      <word>uncurry</word>
      <word>truncate</word>
      <word>True</word>
      <word>toRational</word>
      <word>toInteger</word>
      <word>toEnum</word>
      <word>tanh</word>
      <word>tan</word>
      <word>takeWhile</word>
      <word>take</word>
      <word>tail</word>
      <word>sum</word>
      <word>succ</word>
      <word>subtract</word>
      <word>String</word>
      <word>sqrt</word>
      <word>splitAt</word>
      <word>span</word>
      <word>snd</word>
      <word>sinh</word>
      <word>sin</word>
      <word>signum</word>
      <word>significand</word>
      <word>showString</word>
      <word>showsPrec</word>
      <word>ShowS</word>
      <word>shows</word>
      <word>showParen</word>
      <word>showList</word>
      <word>showChar</word>
      <word>Show</word>
      <word>show</word>
      <word>sequence_</word>
      <word>sequence</word>
      <word>seq</word>
      <word>scanr1</word>
      <word>scanr</word>
      <word>scanl1</word>
      <word>scanl</word>
      <word>scaleFloat</word>
      <word>round</word>
      <word>Right</word>
      <word>reverse</word>
      <word>return</word>
      <word>replicate</word>
      <word>repeat</word>
      <word>rem</word>
      <word>recip</word>
      <word>realToFrac</word>
      <word>RealFrac</word>
      <word>RealFloat</word>
      <word>Real</word>
      <word>readsPrec</word>
      <word>ReadS</word>
      <word>reads</word>
      <word>readParen</word>
      <word>readLn</word>
      <word>readList</word>
      <word>readIO</word>
      <word>readFile</word>
      <word>Read</word>
      <word>read</word>
      <word>quotRem</word>
      <word>quot</word>
      <word>putStrLn</word>
      <word>putStr</word>
      <word>putChar</word>
      <word>properFraction</word>
      <word>product</word>
      <word>print</word>
      <word>pred</word>
      <word>pi</word>
      <word>otherwise</word>
      <word>Ordering</word>
      <word>Ord</word>
      <word>or</word>
      <word>odd</word>
      <word>Num</word>
      <word>null</word>
      <word>Nothing</word>
      <word>notElem</word>
      <word>not</word>
      <word>negate</word>
      <word>Monad</word>
      <word>mod</word>
      <word>minimum</word>
      <word>minBound</word>
      <word>min</word>
      <word>Maybe</word>
      <word>maybe</word>
      <word>maximum</word>
      <word>maxBound</word>
      <word>max</word>
      <word>mapM_</word>
      <word>mapM</word>
      <word>map</word>
      <word>LT</word>
      <word>lookup</word>
      <word>logBase</word>
      <word>log</word>
      <word>lines</word>
      <word>lex</word>
      <word>length</word>
      <word>Left</word>
      <word>lcm</word>
      <word>last</word>
      <word>Just</word>
      <word>iterate</word>
      <word>isNegativeZero</word>
      <word>isNaN</word>
      <word>isInfinite</word>
      <word>isIEEE</word>
      <word>isDenormalized</word>
      <word>IOError</word>
      <word>ioError</word>
      <word>IO</word>
      <word>interact</word>
      <word>Integral</word>
      <word>Integer</word>
      <word>Int</word>
      <word>init</word>
      <word>id</word>
      <word>head</word>
      <word>GT</word>
      <word>getLine</word>
      <word>getContents</word>
      <word>getChar</word>
      <word>gcd</word>
      <word>Functor</word>
      <word>fst</word>
      <word>fromRational</word>
      <word>fromIntegral</word>
      <word>fromInteger</word>
      <word>fromEnum</word>
      <word>Fractional</word>
      <word>foldr1</word>
      <word>foldr</word>
      <word>foldl1</word>
      <word>foldl</word>
      <word>floor</word>
      <word>floatRange</word>
      <word>floatRadix</word>
      <word>Floating</word>
      <word>floatDigits</word>
      <word>Float</word>
      <word>flip</word>
      <word>FilePath</word>
      <word>False</word>
      <word>fail</word>
      <word>exponent</word>
      <word>exp</word>
      <word>even</word>
      <word>error</word>
      <word>EQ</word>
      <word>Eq</word>
      <word>enumFromTo</word>
      <word>enumFromThenTo</word>
      <word>enumFromThen</word>
      <word>enumFrom</word>
      <word>Enum</word>
      <word>encodeFloat</word>
      <word>elem</word>
      <word>Either</word>
      <word>dropWhile</word>
      <word>drop</word>
      <word>Double</word>
      <word>divMod</word>
      <word>div</word>
      <word>decodeFloat</word>
      <word>data</word>
      <word>cycle</word>
      <word>curry</word>
      <word>cosh</word>
      <word>cos</word>
      <word>const</word>
      <word>concatMap</word>
      <word>concat</word>
      <word>compare</word>
      <word>Char</word>
      <word>ceiling</word>
      <word>catch</word>
      <word>break</word>
      <word>Bounded</word>
      <word>Bool</word>
      <word>atanh</word>
      <word>atan2</word>
      <word>atan</word>
      <word>asTypeOf</word>
      <word>asinh</word>
      <word>asin</word>
      <word>applyM</word>
      <word>appendFile</word>
      <word>any</word>
      <word>and</word>
      <word>all</word>
      <word>acosh</word>
      <word>acos</word>
      <word>abs</word>
    </pattern>
  </definition>
  <definition name=""HTML"" caseSensitive=""false"">
    <default>
      <font name=""Menlo"" size=""11"" style=""normal"" foreColor=""black"" backColor=""transparent""/>
    </default>
    <pattern name=""MultiLineComment"" type=""block"" beginsWith=""&amp;lt;!--"" endsWith=""--&amp;gt;"">
      <font name=""Menlo"" size=""11"" style=""normal"" foreColor=""green"" backColor=""transparent""/>
    </pattern>
    <pattern name=""Markup"" type=""markup"" highlightAttributes=""true"">
      <font name=""Menlo"" size=""11"" style=""normal"" foreColor=""maroon"" backColor=""transparent"">
        <bracketStyle foreColor=""blue"" backColor=""transparent""/>
        <attributeNameStyle foreColor=""red"" backColor=""transparent""/>
        <attributeValueStyle foreColor=""blue"" backColor=""transparent""/>
      </font>
    </pattern>
  </definition>
  <definition name=""Java"" caseSensitive=""true"">
    <default>
      <font name=""Menlo"" size=""11"" style=""normal"" foreColor=""black"" backColor=""transparent""/>
    </default>
    <pattern name=""Keyword"" type=""word"">
      <font name=""Menlo"" size=""11"" style=""normal"" foreColor=""blue"" backColor=""transparent""/>
      <word>private</word>
      <word>protected</word>
      <word>public</word>
      <word>namespace</word>
      <word>class</word>
      <word>var</word>
      <word>for</word>
      <word>if</word>
      <word>else</word>
      <word>while</word>
      <word>switch</word>
      <word>case</word>
      <word>using</word>
      <word>get</word>
      <word>return</word>
      <word>null</word>
      <word>void</word>
      <word>int</word>
      <word>string</word>
      <word>float</word>
      <word>this</word>
      <word>set</word>
      <word>new</word>
      <word>true</word>
      <word>false</word>
      <word>const</word>
      <word>static</word>
      <word>package</word>
      <word>function,</word>
      <word>internal</word>
      <word>extends</word>
      <word>super</word>
      <word>import</word>
      <word>default</word>
      <word>break</word>
      <word>try</word>
      <word>catch</word>
      <word>finally</word>
    </pattern>
    <pattern name=""Comment"" type=""block"" beginsWith=""//"" endsWith=""\n"">
      <font name=""Menlo"" size=""11"" style=""normal"" foreColor=""green"" backColor=""transparent""/>
    </pattern>
    <pattern name=""MultiLineComment"" type=""block"" beginsWith=""/*"" endsWith=""*/"">
      <font name=""Menlo"" size=""11"" style=""normal"" foreColor=""green"" backColor=""transparent""/>
    </pattern>
    <pattern name=""Operators"" type=""word"">
      <font name=""Menlo"" size=""11"" style=""normal"" foreColor=""red"" backColor=""transparent""/>
      <word>+</word>
      <word>-</word>
      <word>=</word>
    </pattern>
  </definition>
  <definition name=""JavaScript"" caseSensitive=""true"">
    <default>
      <font name=""Menlo"" size=""11"" style=""normal"" foreColor=""black"" backColor=""transparent""/>
    </default>
    <pattern name=""Function"" type=""word"">
      <font name=""Menlo"" size=""11"" style=""normal"" foreColor=""purple"" backColor=""transparent""/>
      <word>GetObject</word>
      <word>ScriptEngine</word>
      <word>ScriptEngineBuildVersion</word>
      <word>ScriptEngineMajorVersion</word>
      <word>ScriptEngineMinorVersion</word>
    </pattern>
    <pattern name=""Method"" type=""word"">
      <font name=""Menlo"" size=""11"" style=""normal"" foreColor=""blue"" backColor=""transparent""/>
      <word>abs</word>
      <word>acos</word>
      <word>anchor</word>
      <word>apply</word>
      <word>asin</word>
      <word>atan</word>
      <word>atan2</word>
      <word>atEnd</word>
      <word>big</word>
      <word>blink</word>
      <word>bold</word>
      <word>call</word>
      <word>ceil</word>
      <word>charAt</word>
      <word>charCodeAt</word>
      <word>compile</word>
      <word>concat</word>
      <word>cos</word>
      <word>decodeURI</word>
      <word>decodeURIComponent</word>
      <word>dimensions</word>
      <word>encodeURI</word>
      <word>encodeURIComponent</word>
      <word>escape</word>
      <word>eval</word>
      <word>exec</word>
      <word>exp</word>
      <word>fixed</word>
      <word>floor</word>
      <word>fontcolor</word>
      <word>fontsize</word>
      <word>fromCharCode</word>
      <word>getDate</word>
      <word>getDay</word>
      <word>getFullYear</word>
      <word>getHours</word>
      <word>getItem</word>
      <word>getMilliseconds</word>
      <word>getMinutes</word>
      <word>getMonth</word>
      <word>getSeconds</word>
      <word>getTime</word>
      <word>getTimezoneOffset</word>
      <word>getUTCDate</word>
      <word>getUTCDay</word>
      <word>getUTCFullYear</word>
      <word>getUTCHours</word>
      <word>getUTCMilliseconds</word>
      <word>getUTCMinutes</word>
      <word>getUTCMonth</word>
      <word>getUTCSeconds</word>
      <word>getVarDate</word>
      <word>getYear</word>
      <word>hasOwnProperty</word>
      <word>IndexOf</word>
      <word>isFinite</word>
      <word>isNaN</word>
      <word>isPrototypeOf</word>
      <word>italics</word>
      <word>item</word>
      <word>join</word>
      <word>lastIndexOf</word>
      <word>lbound</word>
      <word>link</word>
      <word>localeCompare</word>
      <word>log</word>
      <word>match</word>
      <word>max</word>
      <word>min</word>
      <word>moveFirst</word>
      <word>moveNext</word>
      <word>parse</word>
      <word>parseFloat</word>
      <word>parseInt</word>
      <word>pop</word>
      <word>pow</word>
      <word>push</word>
      <word>random</word>
      <word>replace</word>
      <word>reverse</word>
      <word>round</word>
      <word>search</word>
      <word>setDate</word>
      <word>setFullYear</word>
      <word>setHours</word>
      <word>setMilliseconds</word>
      <word>setMinutes</word>
      <word>setMonth</word>
      <word>setSeconds</word>
      <word>setTime</word>
      <word>setUTCDate</word>
      <word>setUTCFullYear</word>
      <word>setUTCHours</word>
      <word>setUTCMilliseconds</word>
      <word>setUTCMinutes</word>
      <word>setUTCMonth</word>
      <word>setUTCSeconds</word>
      <word>setYear</word>
      <word>shift</word>
      <word>sin</word>
      <word>slice</word>
      <word>small</word>
      <word>sort</word>
      <word>splice</word>
      <word>split</word>
      <word>sqrt</word>
      <word>strike</word>
      <word>sub</word>
      <word>substr</word>
      <word>substring</word>
      <word>sup</word>
      <word>tan</word>
      <word>test</word>
      <word>toArray</word>
      <word>toDateString</word>
      <word>toExponential</word>
      <word>toFixed</word>
      <word>toGMTString</word>
      <word>toLocaleDateString</word>
      <word>toLocaleLowerCase</word>
      <word>toLocaleString</word>
      <word>toLocaleTimeString</word>
      <word>toLocaleUpperCase</word>
      <word>toLowerCase</word>
      <word>toPrecision</word>
      <word>toString</word>
      <word>toTimeString</word>
      <word>toUpperCase</word>
      <word>toUTCString</word>
      <word>ubound</word>
      <word>unescape</word>
      <word>unshift</word>
      <word>UTC</word>
      <word>valueOf</word>
      <word>write</word>
      <word>writeln</word>
    </pattern>
    <pattern name=""Object"" type=""word"">
      <font name=""Menlo"" size=""11"" style=""normal"" foreColor=""red"" backColor=""transparent""/>
      <word>ActiveXObject</word>
      <word>Array</word>
      <word>arguments</word>
      <word>Boolean</word>
      <word>Date</word>
      <word>Debug</word>
      <word>Enumerator</word>
      <word>Error</word>
      <word>Function</word>
      <word>Global</word>
      <word>Math</word>
      <word>Number</word>
      <word>Object</word>
      <word>RegExp</word>
      <word>String</word>
      <word>VBArray</word>
    </pattern>
    <pattern name=""Statement"" type=""word"">
      <font name=""Menlo"" size=""11"" style=""normal"" foreColor=""blue"" backColor=""transparent""/>
      <word>if</word>
      <word>else</word>
      <word>switch</word>
      <word>case</word>
      <word>do</word>
      <word>for</word>
      <word>function</word>
      <word>in</word>
      <word>while</word>
      <word>break</word>
      <word>continue</word>
      <word>return</word>
      <word>try</word>
      <word>throw</word>
      <word>catch</word>
      <word>finally</word>
      <word>checked</word>
      <word>unchecked</word>
      <word>fixed</word>
      <word>lock</word>
      <word>this</word>
      <word>var</word>
      <word>with</word>
      <word>@cc_on</word>
      <word>@if</word>
      <word>@set</word>
      <word>@elif</word>
      <word>@else</word>
      <word>@end</word>
    </pattern>
    <pattern name=""Operator"" type=""word"">
      <font name=""Menlo"" size=""11"" style=""normal"" foreColor=""red"" backColor=""transparent""/>
      <word>+</word>
      <word>-</word>
      <word>=</word>
      <word>%</word>
      <word>*</word>
      <word>/</word>
      <word>&amp;</word>
      <word>|</word>
    </pattern>
    <pattern name=""Property"" type=""word"">
      <font name=""Menlo"" size=""11"" style=""normal"" foreColor=""red"" backColor=""transparent""/>
      <word>arguments</word>
      <word>callee</word>
      <word>caller</word>
      <word>constructor</word>
      <word>description</word>
      <word>E</word>
      <word>global</word>
      <word>ignoreCase</word>
      <word>index</word>
      <word>Infinity</word>
      <word>input</word>
      <word>lastIndex</word>
      <word>lastParent</word>
      <word>leftContext</word>
      <word>length</word>
      <word>LN2</word>
      <word>LN10</word>
      <word>LOG2E</word>
      <word>LOG10E</word>
      <word>MAX</word>
      <word>message</word>
      <word>MIN</word>
      <word>multiline</word>
      <word>name</word>
      <word>NaN</word>
      <word>NEGATIVE_INFINITY</word>
      <word>number</word>
      <word>PI</word>
      <word>POSITIVE_INFINITY</word>
      <word>propertyIsEnumerable</word>
      <word>prototype</word>
      <word>rightContext</word>
      <word>source</word>
      <word>SQRT1_2</word>
      <word>SQRT2</word>
      <word>undefined</word>
    </pattern>
    <pattern name=""String"" type=""block"" beginsWith=""&quot;"" endsWith=""&quot;"" escapesWith=""\"">
      <font name=""Menlo"" size=""11"" style=""normal"" foreColor=""#666666"" backColor=""transparent""/>
    </pattern>
    <pattern name=""Comment"" type=""block"" beginsWith=""//"" endsWith=""\n"">
      <font name=""Menlo"" size=""11"" style=""normal"" foreColor=""green"" backColor=""transparent""/>
    </pattern>
    <pattern name=""MultiLineComment"" type=""block"" beginsWith=""/*"" endsWith=""*/"">
      <font name=""Menlo"" size=""11"" style=""normal"" foreColor=""green"" backColor=""transparent""/>
    </pattern>
  </definition>
  <definition name=""Mercury"" caseSensitive=""true"">
    <default>
      <font name=""Menlo"" size=""11"" style=""normal"" foreColor=""black"" backColor=""transparent""/>
    </default>
    <pattern name=""Comment"" type=""block"" beginsWith=""//"" endsWith=""\n"">
      <font name=""Menlo"" size=""11"" style=""normal"" foreColor=""green"" backColor=""transparent""/>
    </pattern>
    <pattern name=""MultiLineComment"" type=""block"" beginsWith=""/*"" endsWith=""*/"">
      <font name=""Menlo"" size=""11"" style=""normal"" foreColor=""green"" backColor=""transparent""/>
    </pattern>
    <pattern name=""String"" type=""block"" beginsWith=""&quot;"" endsWith=""&quot;"" escapesWith=""\"">
      <font name=""Menlo"" size=""11"" style=""normal"" foreColor=""darkred"" backColor=""transparent""/>
    </pattern>
    <pattern name=""WordGroup01"" type=""word"">
      <font name=""Menlo"" size=""11"" style=""normal"" foreColor=""blue"" backColor=""transparent""/>
      <word>where</word>
      <word>when</word>
      <word>use_module</word>
      <word>use_module</word>
      <word>typeclass</word>
      <word>typeclass</word>
      <word>type</word>
      <word>type</word>
      <word>then</word>
      <word>some</word>
      <word>semipure</word>
      <word>rule</word>
      <word>rem</word>
      <word>promise</word>
      <word>promise</word>
      <word>pred</word>
      <word>pred</word>
      <word>pragma</word>
      <word>pragma</word>
      <word>or</word>
      <word>not</word>
      <word>module</word>
      <word>module</word>
      <word>mode</word>
      <word>mode</word>
      <word>mod</word>
      <word>lambda</word>
      <word>is</word>
      <word>interface</word>
      <word>instance</word>
      <word>instance</word>
      <word>inst</word>
      <word>inst</word>
      <word>include_module</word>
      <word>include_module</word>
      <word>impure</word>
      <word>import_module</word>
      <word>import_module</word>
      <word>implementation</word>
      <word>if</word>
      <word>func</word>
      <word>func</word>
      <word>end_module</word>
      <word>end_module</word>
      <word>else</word>
      <word>div</word>
      <word>and</word>
      <word>all</word>
      <word>aditi_top_down</word>
      <word>aditi_bottom_up</word>
    </pattern>
  </definition>
  <definition name=""MSIL"" caseSensitive=""true"">
    <default>
      <font name=""Menlo"" size=""11"" style=""normal"" foreColor=""black"" backColor=""transparent""/>
    </default>
    <pattern name=""PreprocessorDirective"" type=""word"">
      <font name=""Menlo"" size=""11"" style=""normal"" foreColor=""green"" backColor=""transparent""/>
      <word>.addon</word>
      <word>.algorithm</word>
      <word>.assembly</word>
      <word>.backing</word>
      <word>.blob</word>
      <word>.capability</word>
      <word>.cctor</word>
      <word>.class</word>
      <word>.comtype</word>
      <word>.config</word>
      <word>.corflags</word>
      <word>.ctor</word>
      <word>.custom</word>
      <word>.data</word>
      <word>.emitbyte</word>
      <word>.entrypoint</word>
      <word>.event</word>
      <word>.exeloc</word>
      <word>.export</word>
      <word>.field</word>
      <word>.file</word>
      <word>.fire</word>
      <word>.get</word>
      <word>.hash</word>
      <word>.imagebase</word>
      <word>.implicitcom</word>
      <word>.library</word>
      <word>.line</word>
      <word>#line</word>
      <word>.locale</word>
      <word>.locals</word>
      <word>.manifestres</word>
      <word>.maxstack</word>
      <word>.method</word>
      <word>.mime</word>
      <word>.module</word>
      <word>.mresource</word>
      <word>.namespace</word>
      <word>.originator</word>
      <word>.os</word>
      <word>.other</word>
      <word>.override</word>
      <word>.pack</word>
      <word>.param</word>
      <word>.permission</word>
      <word>.permissionset</word>
      <word>.processor</word>
      <word>.property</word>
      <word>.publickey</word>
      <word>.publickeytoken</word>
      <word>.removeon</word>
      <word>.set</word>
      <word>.size</word>
      <word>.subsystem</word>
      <word>.title</word>
      <word>.try</word>
      <word>.ver</word>
      <word>.vtable</word>
      <word>.vtentry</word>
      <word>.vtfixup</word>
      <word>.zeroinit</word>
    </pattern>
    <pattern name=""Keywords"" type=""word"">
      <font name=""Menlo"" size=""11"" style=""normal"" foreColor=""blue"" backColor=""transparent""/>
      <word>abstract</word>
      <word>algorithm</word>
      <word>alignment</word>
      <word>ansi</word>
      <word>any</word>
      <word>array</word>
      <word>as</word>
      <word>assembly</word>
      <word>assert</word>
      <word>at</word>
      <word>auto</word>
      <word>autochar</word>
      <word>beforefieldinit</word>
      <word>blob</word>
      <word>blob_object</word>
      <word>bool</word>
      <word>boxed</word>
      <word>bstr</word>
      <word>bytearray</word>
      <word>byvalstr</word>
      <word>callmostderived</word>
      <word>carray</word>
      <word>catch</word>
      <word>cdecl</word>
      <word>cf</word>
      <word>char</word>
      <word>cil</word>
      <word>class</word>
      <word>clsid</word>
      <word>compilercontrolled</word>
      <word>currency</word>
      <word>custom</word>
      <word>date</word>
      <word>decimal</word>
      <word>default</word>
      <word>demand</word>
      <word>deny</word>
      <word>disablejitoptimizer</word>
      <word>enablejittracking</word>
      <word>error</word>
      <word>explicit</word>
      <word>extends</word>
      <word>extern</word>
      <word>false</word>
      <word>famandassem</word>
      <word>family</word>
      <word>famorassem</word>
      <word>fastcall</word>
      <word>fault</word>
      <word>field</word>
      <word>filetime</word>
      <word>filter</word>
      <word>final</word>
      <word>finally</word>
      <word>fixed</word>
      <word>float</word>
      <word>float32</word>
      <word>float64</word>
      <word>forwardref</word>
      <word>fromunmanaged</word>
      <word>fullorigin</word>
      <word>handler</word>
      <word>hidebysig</word>
      <word>hresult</word>
      <word>idispatch</word>
      <word>il</word>
      <word>implements</word>
      <word>implicitcom</word>
      <word>implicitres</word>
      <word>import</word>
      <word>in</word>
      <word>inf</word>
      <word>inheritcheck</word>
      <word>init</word>
      <word>initonly</word>
      <word>instance</word>
      <word>int</word>
      <word>int16</word>
      <word>int32</word>
      <word>int64</word>
      <word>int8</word>
      <word>interface</word>
      <word>internalcall</word>
      <word>iunknown</word>
      <word>java</word>
      <word>lasterr</word>
      <word>lateinit</word>
      <word>lcid</word>
      <word>linkcheck</word>
      <word>literal</word>
      <word>lpstr</word>
      <word>lpstruct</word>
      <word>lptstr</word>
      <word>lpvoid</word>
      <word>lpwstr</word>
      <word>managed</word>
      <word>marshal</word>
      <word>method</word>
      <word>modopt</word>
      <word>modreq</word>
      <word>nan</word>
      <word>native</word>
      <word>nested</word>
      <word>newslot</word>
      <word>noappdomain</word>
      <word>noinlining</word>
      <word>nomachine</word>
      <word>nomangle</word>
      <word>nometadata</word>
      <word>noncasdemand</word>
      <word>noncasinheritance</word>
      <word>noncaslinkdemand</word>
      <word>noprocess</word>
      <word>not_in_gc_heap</word>
      <word>notserialized</word>
      <word>null</word>
      <word>object</word>
      <word>objectref</word>
      <word>ole</word>
      <word>opt</word>
      <word>optil</word>
      <word>out</word>
      <word>permitonly</word>
      <word>pinned</word>
      <word>pinvokeimpl</word>
      <word>prejitdeny</word>
      <word>prejitgrant</word>
      <word>preservesig</word>
      <word>private</word>
      <word>privatescope</word>
      <word>public</word>
      <word>publickey</word>
      <word>readonly</word>
      <word>record</word>
      <word>reqmin</word>
      <word>reqopt</word>
      <word>reqrefuse</word>
      <word>reqsecobj</word>
      <word>request</word>
      <word>retval</word>
      <word>rtspecialname</word>
      <word>runtime</word>
      <word>safearray</word>
      <word>sealed</word>
      <word>sequential</word>
      <word>serializable</word>
      <word>specialname</word>
      <word>static</word>
      <word>stdcall</word>
      <word>storage</word>
      <word>stored_object</word>
      <word>stream</word>
      <word>streamed_object</word>
      <word>string</word>
      <word>struct</word>
      <word>synchronized</word>
      <word>syschar</word>
      <word>sysstring</word>
      <word>tbstr</word>
      <word>thiscall</word>
      <word>tls</word>
      <word>to</word>
      <word>true</word>
      <word>typedref</word>
      <word>unicode</word>
      <word>unmanaged</word>
      <word>unmanagedexp</word>
      <word>unsigned</word>
      <word>userdefined</word>
      <word>value</word>
      <word>valuetype</word>
      <word>vararg</word>
      <word>variant</word>
      <word>vector</word>
      <word>virtual</word>
      <word>void</word>
      <word>wchar</word>
      <word>winapi</word>
      <word>with</word>
    </pattern>
    <pattern name=""Instructions"" type=""word"">
      <font name=""Menlo"" size=""11"" style=""normal"" foreColor=""blue"" backColor=""transparent""/>
      <word>add</word>
      <word>add.ovf</word>
      <word>add.ovf.un</word>
      <word>and</word>
      <word>ann.call</word>
      <word>ann.catch</word>
      <word>ann.data</word>
      <word>ann.data.s</word>
      <word>ann.dead</word>
      <word>ann.def</word>
      <word>ann.hoisted</word>
      <word>ann.hoisted_call</word>
      <word>ann.lab</word>
      <word>ann.live</word>
      <word>ann.phi</word>
      <word>ann.ref</word>
      <word>ann.ref.s</word>
      <word>arglist</word>
      <word>beq</word>
      <word>beq.s</word>
      <word>bge</word>
      <word>bge.s</word>
      <word>bge.un</word>
      <word>bge.un.s</word>
      <word>bgt</word>
      <word>bgt.s</word>
      <word>bgt.un</word>
      <word>bgt.un.s</word>
      <word>ble</word>
      <word>ble.s</word>
      <word>ble.un</word>
      <word>ble.un.s</word>
      <word>blt</word>
      <word>blt.s</word>
      <word>blt.un</word>
      <word>blt.un.s</word>
      <word>bne.un</word>
      <word>bne.un.s</word>
      <word>box</word>
      <word>box_old</word>
      <word>br</word>
      <word>break</word>
      <word>brfalse</word>
      <word>brfalse.s</word>
      <word>brinst</word>
      <word>brinst.s</word>
      <word>brnull</word>
      <word>brnull.s</word>
      <word>br.s</word>
      <word>brtrue</word>
      <word>brtrue.s</word>
      <word>brzero</word>
      <word>brzero.s</word>
      <word>call</word>
      <word>calli</word>
      <word>callvirt</word>
      <word>castclass</word>
      <word>ceq</word>
      <word>cgt</word>
      <word>cgt.un</word>
      <word>ckfinite</word>
      <word>clt</word>
      <word>clt.un</word>
      <word>conv.i</word>
      <word>conv.i1</word>
      <word>conv.i2</word>
      <word>conv.i4</word>
      <word>conv.i8</word>
      <word>conv.ovf.i</word>
      <word>conv.ovf.i1</word>
      <word>conv.ovf.i1.un</word>
      <word>conv.ovf.i2</word>
      <word>conv.ovf.i2.un</word>
      <word>conv.ovf.i4</word>
      <word>conv.ovf.i4.un</word>
      <word>conv.ovf.i8</word>
      <word>conv.ovf.i8.un</word>
      <word>conv.ovf.i.un</word>
      <word>conv.ovf.u</word>
      <word>conv.ovf.u1</word>
      <word>conv.ovf.u1.un</word>
      <word>conv.ovf.u2</word>
      <word>conv.ovf.u2.un</word>
      <word>conv.ovf.u4</word>
      <word>conv.ovf.u4.un</word>
      <word>conv.ovf.u8</word>
      <word>conv.ovf.u8.un</word>
      <word>conv.ovf.u.un</word>
      <word>conv.r4</word>
      <word>conv.r8</word>
      <word>conv.r.un</word>
      <word>conv.u</word>
      <word>conv.u1</word>
      <word>conv.u2</word>
      <word>conv.u4</word>
      <word>conv.u8</word>
      <word>cpblk</word>
      <word>cpobj</word>
      <word>div</word>
      <word>div.un</word>
      <word>dup</word>
      <word>endfault</word>
      <word>endfilter</word>
      <word>endfinally</word>
      <word>initblk</word>
      <word>initobj</word>
      <word>isinst</word>
      <word>jmp</word>
      <word>jmpi</word>
      <word>ldarg</word>
      <word>ldarg.0</word>
      <word>ldarg.1</word>
      <word>ldarg.2</word>
      <word>ldarg.3</word>
      <word>ldarga</word>
      <word>ldarga.s</word>
      <word>ldarg.s</word>
      <word>ldc.i4</word>
      <word>ldc.i4.0</word>
      <word>ldc.i4.1</word>
      <word>ldc.i4.2</word>
      <word>ldc.i4.3</word>
      <word>ldc.i4.4</word>
      <word>ldc.i4.5</word>
      <word>ldc.i4.6</word>
      <word>ldc.i4.7</word>
      <word>ldc.i4.8</word>
      <word>ldc.i4.m1</word>
      <word>ldc.i4.M1</word>
      <word>ldc.i4.s</word>
      <word>ldc.i8</word>
      <word>ldc.r4</word>
      <word>ldc.r8</word>
      <word>ldelema</word>
      <word>ldelem.i</word>
      <word>ldelem.i1</word>
      <word>ldelem.i2</word>
      <word>ldelem.i4</word>
      <word>ldelem.i8</word>
      <word>ldelem.r4</word>
      <word>ldelem.r8</word>
      <word>ldelem.ref</word>
      <word>ldelem.u1</word>
      <word>ldelem.u2</word>
      <word>ldelem.u4</word>
      <word>ldelem.u8</word>
      <word>ldfld</word>
      <word>ldflda</word>
      <word>ldftn</word>
      <word>ldind.i</word>
      <word>ldind.i1</word>
      <word>ldind.i2</word>
      <word>ldind.i4</word>
      <word>ldind.i8</word>
      <word>ldind.r4</word>
      <word>ldind.r8</word>
      <word>ldind.ref</word>
      <word>ldind.u1</word>
      <word>ldind.u2</word>
      <word>ldind.u4</word>
      <word>ldind.u8</word>
      <word>ldlen</word>
      <word>ldloc</word>
      <word>ldloc.0</word>
      <word>ldloc.1</word>
      <word>ldloc.2</word>
      <word>ldloc.3</word>
      <word>ldloca</word>
      <word>ldloca.s</word>
      <word>ldloc.s</word>
      <word>ldnull</word>
      <word>ldobj</word>
      <word>ldptr</word>
      <word>ldsfld</word>
      <word>ldsflda</word>
      <word>ldstr</word>
      <word>ldtoken</word>
      <word>ldvirtftn</word>
      <word>leave</word>
      <word>leave.s</word>
      <word>localloc</word>
      <word>mkrefany</word>
      <word>mul</word>
      <word>mul.ovf</word>
      <word>mul.ovf.un</word>
      <word>mul.un</word>
      <word>neg</word>
      <word>newarr</word>
      <word>newobj</word>
      <word>nop</word>
      <word>not</word>
      <word>or</word>
      <word>pop</word>
      <word>refanytype</word>
      <word>refanyval</word>
      <word>rem</word>
      <word>rem.un</word>
      <word>ret</word>
      <word>rethrow</word>
      <word>shl</word>
      <word>shr</word>
      <word>shr.un</word>
      <word>sizeof</word>
      <word>starg</word>
      <word>starg.s</word>
      <word>stelem.i</word>
      <word>stelem.i1</word>
      <word>stelem.i2</word>
      <word>stelem.i4</word>
      <word>stelem.i8</word>
      <word>stelem.r4</word>
      <word>stelem.r8</word>
      <word>stelem.ref</word>
      <word>stfld</word>
      <word>stind.i</word>
      <word>stind.i1</word>
      <word>stind.i2</word>
      <word>stind.i4</word>
      <word>stind.i8</word>
      <word>stind.r4</word>
      <word>stind.r8</word>
      <word>stind.ref</word>
      <word>stloc</word>
      <word>stloc.0</word>
      <word>stloc.1</word>
      <word>stloc.2</word>
      <word>stloc.3</word>
      <word>stloc.s</word>
      <word>stobj</word>
      <word>stsfld</word>
      <word>sub</word>
      <word>sub.ovf</word>
      <word>sub.ovf.un</word>
      <word>switch</word>
      <word>tail</word>
      <word>tail.</word>
      <word>throw</word>
      <word>unbox</word>
      <word>volatile</word>
      <word>volatile.</word>
      <word>xor</word>
    </pattern>
    <pattern name=""Operator"" type=""word"">
      <font name=""Menlo"" size=""11"" style=""normal"" foreColor=""red"" backColor=""transparent""/>
      <word>+</word>
      <word>-</word>
      <word>=</word>
      <word>%</word>
      <word>*</word>
      <word>/</word>
      <word>&amp;</word>
      <word>~</word>
      <word>|</word>
      <word>^</word>
      <word>==</word>
    </pattern>
    <pattern name=""String"" type=""block"" beginsWith=""&quot;"" endsWith=""&quot;"" escapesWith=""\"">
      <font name=""Menlo"" size=""11"" style=""normal"" foreColor=""#666666"" backColor=""transparent""/>
    </pattern>
    <pattern name=""Comment"" type=""block"" beginsWith=""//"" endsWith=""\n"">
      <font name=""Menlo"" size=""11"" style=""normal"" foreColor=""green"" backColor=""transparent""/>
    </pattern>
    <pattern name=""char"" type=""block"" beginsWith=""'"" endsWith=""'"" escapesWith=""\"">
      <font name=""Menlo"" size=""11"" style=""normal"" foreColor=""red"" backColor=""transparent""/>
    </pattern>
  </definition>
  <definition name=""Pascal"" caseSensitive=""true"">
    <default>
      <font name=""Menlo"" size=""11"" style=""normal"" foreColor=""black"" backColor=""transparent""/>
    </default>
    <pattern name=""Comment"" type=""block"" beginsWith=""//"" endsWith=""\n"">
      <font name=""Menlo"" size=""11"" style=""normal"" foreColor=""green"" backColor=""transparent""/>
    </pattern>
    <pattern name=""MultiLineComment"" type=""block"" beginsWith=""/*"" endsWith=""*/"">
      <font name=""Menlo"" size=""11"" style=""normal"" foreColor=""green"" backColor=""transparent""/>
    </pattern>
    <pattern name=""String"" type=""block"" beginsWith=""&quot;"" endsWith=""&quot;"" escapesWith=""\"">
      <font name=""Menlo"" size=""11"" style=""normal"" foreColor=""darkred"" backColor=""transparent""/>
    </pattern>
    <pattern name=""WordGroup01"" type=""word"">
      <font name=""Menlo"" size=""11"" style=""normal"" foreColor=""blue"" backColor=""transparent""/>
      <word>xor</word>
      <word>with</word>
      <word>while</word>
      <word>virtual</word>
      <word>var</word>
      <word>uses</word>
      <word>until</word>
      <word>unit</word>
      <word>type</word>
      <word>to</word>
      <word>then</word>
      <word>string</word>
      <word>stdcall</word>
      <word>shr</word>
      <word>shl</word>
      <word>set</word>
      <word>repeat</word>
      <word>record</word>
      <word>program</word>
      <word>procedure</word>
      <word>packed</word>
      <word>overload</word>
      <word>os2call</word>
      <word>or</word>
      <word>of</word>
      <word>object</word>
      <word>not</word>
      <word>nil</word>
      <word>name</word>
      <word>mod</word>
      <word>library</word>
      <word>label</word>
      <word>interface</word>
      <word>inline</word>
      <word>inherited</word>
      <word>Index</word>
      <word>in</word>
      <word>implementation</word>
      <word>if</word>
      <word>goto</word>
      <word>function</word>
      <word>Forward</word>
      <word>for</word>
      <word>file</word>
      <word>external</word>
      <word>exports</word>
      <word>Export</word>
      <word>end</word>
      <word>else</word>
      <word>downto</word>
      <word>do</word>
      <word>div</word>
      <word>destructor</word>
      <word>Declare</word>
      <word>declare</word>
      <word>Conv</word>
      <word>constructor</word>
      <word>const</word>
      <word>Code</word>
      <word>Cdecl</word>
      <word>case</word>
      <word>begin</word>
      <word>Assembler</word>
      <word>asm</word>
      <word>array</word>
      <word>and</word>
      <word>Absolute</word>
    </pattern>
  </definition>
  <definition name=""Perl"" caseSensitive=""true"">
    <default>
      <font name=""Menlo"" size=""11"" style=""normal"" foreColor=""black"" backColor=""transparent""/>
    </default>
    <pattern name=""Comment"" type=""block"" beginsWith=""//"" endsWith=""\n"">
      <font name=""Menlo"" size=""11"" style=""normal"" foreColor=""green"" backColor=""transparent""/>
    </pattern>
    <pattern name=""MultiLineComment"" type=""block"" beginsWith=""/*"" endsWith=""*/"">
      <font name=""Menlo"" size=""11"" style=""normal"" foreColor=""green"" backColor=""transparent""/>
    </pattern>
    <pattern name=""String"" type=""block"" beginsWith=""&quot;"" endsWith=""&quot;"" escapesWith=""\"">
      <font name=""Menlo"" size=""11"" style=""normal"" foreColor=""darkred"" backColor=""transparent""/>
    </pattern>
    <pattern name=""WordGroup01"" type=""word"">
      <font name=""Menlo"" size=""11"" style=""normal"" foreColor=""blue"" backColor=""transparent""/>
      <word>WRITE</word>
      <word>write</word>
      <word>warn</word>
      <word>wantarray</word>
      <word>waitpid</word>
      <word>wait</word>
      <word>vec</word>
      <word>values</word>
      <word>value</word>
      <word>utime</word>
      <word>USER</word>
      <word>use</word>
      <word>UNTIE</word>
      <word>untie</word>
      <word>UNSHIFT</word>
      <word>unshift</word>
      <word>unpack</word>
      <word>unlink</word>
      <word>undef</word>
      <word>umask</word>
      <word>ucfirst</word>
      <word>uc</word>
      <word>truncate</word>
      <word>times</word>
      <word>time</word>
      <word>TIESCALAR</word>
      <word>TIEHASH</word>
      <word>TIEHANDLE</word>
      <word>tied</word>
      <word>TIEARRAY</word>
      <word>tie</word>
      <word>telldir</word>
      <word>tell</word>
      <word>syswrite</word>
      <word>system</word>
      <word>sysseek</word>
      <word>sysread</word>
      <word>sysopen</word>
      <word>syscall</word>
      <word>symlink</word>
      <word>substr</word>
      <word>sub</word>
      <word>study</word>
      <word>STORESIZE</word>
      <word>STORE</word>
      <word>stat</word>
      <word>srand</word>
      <word>sqrt</word>
      <word>split</word>
      <word>SPLICE</word>
      <word>splice</word>
      <word>sort</word>
      <word>socketpair</word>
      <word>socket</word>
      <word>sleep</word>
      <word>sin</word>
      <word>shutdown</word>
      <word>shmwrite</word>
      <word>shmread</word>
      <word>shmget</word>
      <word>shmctl</word>
      <word>SHIFT</word>
      <word>shift</word>
      <word>setsockopt</word>
      <word>setservent</word>
      <word>setpwent</word>
      <word>setprotoent</word>
      <word>setpriority</word>
      <word>setpgrp</word>
      <word>setnetent</word>
      <word>sethostent</word>
      <word>setgrent</word>
      <word>send</word>
      <word>semop</word>
      <word>semget</word>
      <word>semctl</word>
      <word>select</word>
      <word>seekdir</word>
      <word>seek</word>
      <word>scalar</word>
      <word>rmdir</word>
      <word>rindex</word>
      <word>rewinddir</word>
      <word>reverse</word>
      <word>return</word>
      <word>reset</word>
      <word>require</word>
      <word>rename</word>
      <word>ref</word>
      <word>redo</word>
      <word>recv</word>
      <word>readpipe</word>
      <word>readlink</word>
      <word>READLINE</word>
      <word>readline</word>
      <word>readdir</word>
      <word>READ</word>
      <word>read</word>
      <word>rand</word>
      <word>quotemeta</word>
      <word>quotemeta</word>
      <word>PUSH</word>
      <word>push</word>
      <word>prototype</word>
      <word>PRINTF</word>
      <word>printf</word>
      <word>PRINT</word>
      <word>print</word>
      <word>pos</word>
      <word>POP</word>
      <word>pop</word>
      <word>pipe</word>
      <word>package</word>
      <word>pack</word>
      <word>our</word>
      <word>ord</word>
      <word>opendir</word>
      <word>open</word>
      <word>offset</word>
      <word>oct</word>
      <word>no</word>
      <word>NEXTKEY</word>
      <word>next</word>
      <word>my</word>
      <word>msgsnd</word>
      <word>msgrcv</word>
      <word>msgget</word>
      <word>msgctl</word>
      <word>Module</word>
      <word>mkdir</word>
      <word>map</word>
      <word>lstat</word>
      <word>log</word>
      <word>log</word>
      <word>lock</word>
      <word>localtime</word>
      <word>local</word>
      <word>listen</word>
      <word>LIST</word>
      <word>link</word>
      <word>length</word>
      <word>lcfirst</word>
      <word>lc</word>
      <word>lastkey</word>
      <word>last</word>
      <word>kill</word>
      <word>keys</word>
      <word>key</word>
      <word>join</word>
      <word>ioctl</word>
      <word>int</word>
      <word>index</word>
      <word>import</word>
      <word>if</word>
      <word>HOME</word>
      <word>hex</word>
      <word>grep</word>
      <word>goto</word>
      <word>gmtime</word>
      <word>glob</word>
      <word>getsockopt</word>
      <word>getsockname</word>
      <word>getservent</word>
      <word>getservbyport</word>
      <word>getservbyname</word>
      <word>getpwuid</word>
      <word>getpwnam</word>
      <word>getpwent</word>
      <word>getprotoent</word>
      <word>getprotobynumber</word>
      <word>getprotobyname</word>
      <word>getpriority</word>
      <word>getppid</word>
      <word>getpgrp</word>
      <word>getpeername</word>
      <word>getnetent</word>
      <word>getnetbyname</word>
      <word>getnetbyaddr</word>
      <word>getlogin</word>
      <word>gethostent</word>
      <word>gethostbyname</word>
      <word>gethostbyaddr</word>
      <word>getgrnam</word>
      <word>getgrgid</word>
      <word>getgrent</word>
      <word>GETC</word>
      <word>getc</word>
      <word>formline</word>
      <word>format</word>
      <word>fork</word>
      <word>flock</word>
      <word>FIRSTKEY</word>
      <word>fileno</word>
      <word>FETCHSIZE</word>
      <word>FETCH</word>
      <word>fcntl</word>
      <word>EXTEND</word>
      <word>exp</word>
      <word>exit</word>
      <word>EXISTS</word>
      <word>exists</word>
      <word>exec</word>
      <word>eval</word>
      <word>eof</word>
      <word>endservent</word>
      <word>endpwent</word>
      <word>endprotoent</word>
      <word>endnetent</word>
      <word>endhostent</word>
      <word>endgrent</word>
      <word>else</word>
      <word>each</word>
      <word>dump</word>
      <word>do</word>
      <word>die</word>
      <word>DESTROY</word>
      <word>DELETE</word>
      <word>delete</word>
      <word>defined</word>
      <word>dbmopen</word>
      <word>dbmclose</word>
      <word>crypt</word>
      <word>cos</word>
      <word>continue</word>
      <word>connect</word>
      <word>closedir</word>
      <word>CLOSE</word>
      <word>close</word>
      <word>CLOBBER</word>
      <word>CLEAR</word>
      <word>chroot</word>
      <word>chr</word>
      <word>chown</word>
      <word>chop</word>
      <word>chomp</word>
      <word>chmod</word>
      <word>chdir</word>
      <word>caller</word>
      <word>bless</word>
      <word>binmode</word>
      <word>bind</word>
      <word>atan2</word>
      <word>alarm</word>
      <word>accept</word>
      <word>abs</word>
    </pattern>
  </definition>
  <definition name=""PHP"" caseSensitive=""false"">
    <default>
      <font name=""Menlo"" size=""11"" style=""normal"" foreColor=""black"" backColor=""transparent""/>
    </default>
    <pattern name=""Comment"" type=""block"" beginsWith=""//"" endsWith=""\n"">
      <font name=""Menlo"" size=""11"" style=""normal"" foreColor=""green"" backColor=""transparent""/>
    </pattern>
    <pattern name=""AlternativeComment"" type=""block"" beginsWith=""#"" endsWith=""\n"">
      <font name=""Menlo"" size=""11"" style=""normal"" foreColor=""green"" backColor=""transparent""/>
    </pattern>
    <pattern name=""MultiLineComment"" type=""block"" beginsWith=""/*"" endsWith=""*/"">
      <font name=""Menlo"" size=""11"" style=""normal"" foreColor=""green"" backColor=""transparent""/>
    </pattern>
    <pattern name=""String"" type=""block"" beginsWith=""&quot;"" endsWith=""&quot;"" escapesWith=""\"">
      <font name=""Menlo"" size=""11"" style=""normal"" foreColor=""darkred"" backColor=""transparent""/>
    </pattern>
    <pattern name=""WordGroup01"" type=""word"">
      <font name=""Menlo"" size=""11"" style=""normal"" foreColor=""blue"" backColor=""transparent""/>
      <word>while</word>
      <word>var</word>
      <word>true</word>
      <word>switch</word>
      <word>return</word>
      <word>require_once</word>
      <word>require</word>
      <word>next</word>
      <word>include_once</word>
      <word>include</word>
      <word>if</word>
      <word>global</word>
      <word>function</word>
      <word>foreach</word>
      <word>for</word>
      <word>false</word>
      <word>extends</word>
      <word>exit</word>
      <word>endwhile</word>
      <word>endswitch</word>
      <word>endif</word>
      <word>endfor</word>
      <word>elseif</word>
      <word>else</word>
      <word>echo</word>
      <word>do</word>
      <word>die</word>
      <word>default</word>
      <word>declare</word>
      <word>continue</word>
      <word>class</word>
      <word>case</word>
      <word>break</word>
      <word>as</word>
    </pattern>
    <pattern name=""WordGroup02"" type=""word"">
      <font name=""Menlo"" size=""11"" style=""normal"" foreColor=""orange"" backColor=""transparent""/>
      <word>zend_version</word>
      <word>zend_test_func</word>
      <word>zend_logo_guid</word>
      <word>yp_order</word>
      <word>yp_next</word>
      <word>yp_match</word>
      <word>yp_master</word>
      <word>yp_get_default_domain</word>
      <word>yp_first</word>
      <word>yaz_wait</word>
      <word>yaz_syntax</word>
      <word>yaz_search</word>
      <word>yaz_record</word>
      <word>yaz_range</word>
      <word>yaz_hits</word>
      <word>yaz_error</word>
      <word>yaz_errno</word>
      <word>yaz_element</word>
      <word>yaz_connect</word>
      <word>yaz_close</word>
      <word>yaz_addinfo</word>
      <word>xslt_transform</word>
      <word>xslt_set_sax_handler</word>
      <word>xslt_set_error_handler</word>
      <word>xslt_run</word>
      <word>xslt_process</word>
      <word>xslt_output_endtransform</word>
      <word>xslt_output_begintransform</word>
      <word>xslt_openlog</word>
      <word>xslt_free</word>
      <word>xslt_fetch_result</word>
      <word>xslt_error</word>
      <word>xslt_errno</word>
      <word>xslt_create</word>
      <word>xslt_closelog</word>
      <word>xptr_new_context</word>
      <word>xptr_eval</word>
      <word>xpath_new_context</word>
      <word>xpath_eval_expression</word>
      <word>xpath_eval</word>
      <word>xmltree</word>
      <word>xmldocfile</word>
      <word>xmldoc</word>
      <word>xml_set_unparsed_entity_decl_handler</word>
      <word>xml_set_processing_instruction_handler</word>
      <word>xml_set_object</word>
      <word>xml_set_notation_decl_handler</word>
      <word>xml_set_external_entity_ref_handler</word>
      <word>xml_set_element_handler</word>
      <word>xml_set_default_handler</word>
      <word>xml_set_character_data_handler</word>
      <word>xml_parser_set_option</word>
      <word>xml_parser_get_option</word>
      <word>xml_parser_free</word>
      <word>xml_parser_create</word>
      <word>xml_parse_into_struct</word>
      <word>xml_parse</word>
      <word>xml_get_error_code</word>
      <word>xml_get_current_line_number</word>
      <word>xml_get_current_column_number</word>
      <word>xml_get_current_byte_index</word>
      <word>xml_error_string</word>
      <word>writev</word>
      <word>write</word>
      <word>wordwrap</word>
      <word>wddx_serialize_vars</word>
      <word>wddx_serialize_value</word>
      <word>wddx_packet_start</word>
      <word>wddx_packet_end</word>
      <word>wddx_deserialize</word>
      <word>wddx_add_vars</word>
      <word>virtual</word>
      <word>velocis_rollback</word>
      <word>velocis_result</word>
      <word>velocis_off_autocommit</word>
      <word>velocis_freeresult</word>
      <word>velocis_fieldnum</word>
      <word>velocis_fieldname</word>
      <word>velocis_fetch</word>
      <word>velocis_exec</word>
      <word>velocis_connect</word>
      <word>velocis_commit</word>
      <word>velocis_close</word>
      <word>velocis_autocommit</word>
      <word>var_dump</word>
      <word>utf8_encode</word>
      <word>utf8_decode</word>
      <word>usort</word>
      <word>usleep</word>
      <word>urlencode</word>
      <word>urldecode</word>
      <word>unset</word>
      <word>unserialize</word>
      <word>unregister_tick_function</word>
      <word>unpack</word>
      <word>unlink</word>
      <word>unixtojd</word>
      <word>uniqid</word>
      <word>umask</word>
      <word>uksort</word>
      <word>ucwords</word>
      <word>ucfirst</word>
      <word>uasort</word>
      <word>trim</word>
      <word>trigger_error</word>
      <word>touch</word>
      <word>tmpfile</word>
      <word>time</word>
      <word>textdomain</word>
      <word>tempnam</word>
      <word>tan</word>
      <word>system</word>
      <word>syslog</word>
      <word>symlink</word>
      <word>sybase_select_db</word>
      <word>sybase_result</word>
      <word>sybase_query</word>
      <word>sybase_pconnect</word>
      <word>sybase_num_rows</word>
      <word>sybase_num_fields</word>
      <word>sybase_min_server_severity</word>
      <word>sybase_min_message_severity</word>
      <word>sybase_min_error_severity</word>
      <word>sybase_min_client_severity</word>
      <word>sybase_get_last_message</word>
      <word>sybase_free_result</word>
      <word>sybase_field_seek</word>
      <word>sybase_fetch_row</word>
      <word>sybase_fetch_object</word>
      <word>sybase_fetch_field</word>
      <word>sybase_fetch_array</word>
      <word>sybase_data_seek</word>
      <word>sybase_connect</word>
      <word>sybase_close</word>
      <word>sybase_affected_rows</word>
      <word>swf_viewport</word>
      <word>swf_translate</word>
      <word>swf_textwidth</word>
      <word>swf_startsymbol</word>
      <word>swf_startshape</word>
      <word>swf_startdoaction</word>
      <word>swf_startbutton</word>
      <word>swf_showframe</word>
      <word>swf_shapemoveto</word>
      <word>swf_shapelineto</word>
      <word>swf_shapelinesolid</word>
      <word>swf_shapefillsolid</word>
      <word>swf_shapefilloff</word>
      <word>swf_shapefillbitmaptile</word>
      <word>swf_shapefillbitmapclip</word>
      <word>swf_shapecurveto3</word>
      <word>swf_shapecurveto</word>
      <word>swf_shapearc</word>
      <word>swf_setframe</word>
      <word>swf_setfont</word>
      <word>swf_scale</word>
      <word>swf_rotate</word>
      <word>swf_removeobject</word>
      <word>swf_pushmatrix</word>
      <word>swf_posround</word>
      <word>swf_popmatrix</word>
      <word>swf_polarview</word>
      <word>swf_placeobject</word>
      <word>swf_perspective</word>
      <word>swf_ortho2</word>
      <word>swf_ortho</word>
      <word>swf_openfile</word>
      <word>swf_oncondition</word>
      <word>swf_nextid</word>
      <word>swf_mulcolor</word>
      <word>swf_modifyobject</word>
      <word>swf_lookat</word>
      <word>swf_labelframe</word>
      <word>swf_getframe</word>
      <word>swf_getfontinfo</word>
      <word>swf_getbitmapinfo</word>
      <word>swf_fonttracking</word>
      <word>swf_fontslant</word>
      <word>swf_fontsize</word>
      <word>swf_endsymbol</word>
      <word>swf_endshape</word>
      <word>swf_enddoaction</word>
      <word>swf_endbutton</word>
      <word>swf_definetext</word>
      <word>swf_definerect</word>
      <word>swf_definepoly</word>
      <word>swf_defineline</word>
      <word>swf_definefont</word>
      <word>swf_definebitmap</word>
      <word>swf_closefile</word>
      <word>swf_addcolor</word>
      <word>swf_addbuttonrecord</word>
      <word>swf_actionwaitforframe</word>
      <word>swf_actiontogglequality</word>
      <word>swf_actionstop</word>
      <word>swf_actionsettarget</word>
      <word>swf_actionprevframe</word>
      <word>swf_actionplay</word>
      <word>swf_actionnextframe</word>
      <word>swf_actiongotolabel</word>
      <word>swf_actiongotoframe</word>
      <word>swf_actiongeturl</word>
      <word>substr_replace</word>
      <word>substr_count</word>
      <word>substr</word>
      <word>strval</word>
      <word>strtr</word>
      <word>strtoupper</word>
      <word>strtotime</word>
      <word>strtolower</word>
      <word>strtok</word>
      <word>strstr</word>
      <word>strspn</word>
      <word>strrpos</word>
      <word>strrev</word>
      <word>strrchr</word>
      <word>strpos</word>
      <word>strncmp</word>
      <word>strncasecmp</word>
      <word>strnatcmp</word>
      <word>strnatcasecmp</word>
      <word>strlen</word>
      <word>stristr</word>
      <word>stripslashes</word>
      <word>stripcslashes</word>
      <word>strip_tags</word>
      <word>strftime</word>
      <word>strerror</word>
      <word>strcspn</word>
      <word>strcmp</word>
      <word>strchr</word>
      <word>strcasecmp</word>
      <word>str_replace</word>
      <word>str_repeat</word>
      <word>str_pad</word>
      <word>stat</word>
      <word>sscanf</word>
      <word>srand</word>
      <word>sqrt</word>
      <word>sql_regcase</word>
      <word>sprintf</word>
      <word>spliti</word>
      <word>split</word>
      <word>soundex</word>
      <word>sort</word>
      <word>socketpair</word>
      <word>socket_set_timeout</word>
      <word>socket_set_blocking</word>
      <word>socket_get_status</word>
      <word>socket</word>
      <word>snmpwalkoid</word>
      <word>snmpwalk</word>
      <word>snmpset</word>
      <word>snmprealwalk</word>
      <word>snmpget</word>
      <word>snmp_set_quick_print</word>
      <word>snmp_get_quick_print</word>
      <word>sleep</word>
      <word>sizeof</word>
      <word>sin</word>
      <word>similar_text</word>
      <word>signal</word>
      <word>shutdown</word>
      <word>shuffle</word>
      <word>show_source</word>
      <word>shmop_write</word>
      <word>shmop_size</word>
      <word>shmop_read</word>
      <word>shmop_open</word>
      <word>shmop_delete</word>
      <word>shmop_close</word>
      <word>shm_remove_var</word>
      <word>shm_remove</word>
      <word>shm_put_var</word>
      <word>shm_get_var</word>
      <word>shm_detach</word>
      <word>shm_attach</word>
      <word>shell_exec</word>
      <word>settype</word>
      <word>setsockopt</word>
      <word>setlocale</word>
      <word>setcookie</word>
      <word>set_time_limit</word>
      <word>set_socket_blocking</word>
      <word>set_nonblock</word>
      <word>set_magic_quotes_runtime</word>
      <word>set_iovec</word>
      <word>set_file_buffer</word>
      <word>set_error_handler</word>
      <word>session_write_close</word>
      <word>session_unset</word>
      <word>session_unregister</word>
      <word>session_start</word>
      <word>session_set_save_handler</word>
      <word>session_set_cookie_params</word>
      <word>session_save_path</word>
      <word>session_register</word>
      <word>session_name</word>
      <word>session_module_name</word>
      <word>session_is_registered</word>
      <word>session_id</word>
      <word>session_get_cookie_params</word>
      <word>session_encode</word>
      <word>session_destroy</word>
      <word>session_decode</word>
      <word>session_cache_limiter</word>
      <word>serialize</word>
      <word>sendto</word>
      <word>sendmsg</word>
      <word>send</word>
      <word>sem_release</word>
      <word>sem_get</word>
      <word>sem_acquire</word>
      <word>select</word>
      <word>satellite_load_idl</word>
      <word>satellite_get_repository_id</word>
      <word>satellite_exception_value</word>
      <word>satellite_exception_id</word>
      <word>satellite_caught_exception</word>
      <word>rtrim</word>
      <word>rsort</word>
      <word>round</word>
      <word>rmdir</word>
      <word>rewinddir</word>
      <word>rewind</word>
      <word>restore_error_handler</word>
      <word>reset</word>
      <word>rename</word>
      <word>register_tick_function</word>
      <word>register_shutdown_function</word>
      <word>recvmsg</word>
      <word>recvfrom</word>
      <word>recv</word>
      <word>recode_string</word>
      <word>recode_file</word>
      <word>recode</word>
      <word>realpath</word>
      <word>readv</word>
      <word>readlink</word>
      <word>readline_write_history</word>
      <word>readline_read_history</word>
      <word>readline_list_history</word>
      <word>readline_info</word>
      <word>readline_completion_function</word>
      <word>readline_clear_history</word>
      <word>readline_add_history</word>
      <word>readline</word>
      <word>readgzfile</word>
      <word>readfile</word>
      <word>readdir</word>
      <word>read_exif_data</word>
      <word>read</word>
      <word>rawurlencode</word>
      <word>rawurldecode</word>
      <word>range</word>
      <word>rand</word>
      <word>rad2deg</word>
      <word>quotemeta</word>
      <word>quoted_printable_decode</word>
      <word>qdom_tree</word>
      <word>putenv</word>
      <word>pspell_suggest</word>
      <word>pspell_store_replacement</word>
      <word>pspell_save_wordlist</word>
      <word>pspell_new_personal</word>
      <word>pspell_new_config</word>
      <word>pspell_new</word>
      <word>pspell_config_save_repl</word>
      <word>pspell_config_runtogether</word>
      <word>pspell_config_repl</word>
      <word>pspell_config_personal</word>
      <word>pspell_config_mode</word>
      <word>pspell_config_ignore</word>
      <word>pspell_config_create</word>
      <word>pspell_clear_session</word>
      <word>pspell_check</word>
      <word>pspell_add_to_session</word>
      <word>pspell_add_to_personal</word>
      <word>printf</word>
      <word>printer_write</word>
      <word>printer_start_page</word>
      <word>printer_start_doc</word>
      <word>printer_set_option</word>
      <word>printer_select_pen</word>
      <word>printer_select_font</word>
      <word>printer_select_brush</word>
      <word>printer_open</word>
      <word>printer_name</word>
      <word>printer_logical_fontheight</word>
      <word>printer_list</word>
      <word>printer_get_option</word>
      <word>printer_end_page</word>
      <word>printer_end_doc</word>
      <word>printer_draw_text</word>
      <word>printer_draw_roundrect</word>
      <word>printer_draw_rectangle</word>
      <word>printer_draw_elipse</word>
      <word>printer_delete_pen</word>
      <word>printer_delete_font</word>
      <word>printer_delete_dc</word>
      <word>printer_delete_brush</word>
      <word>printer_create_pen</word>
      <word>printer_create_font</word>
      <word>printer_create_dc</word>
      <word>printer_create_brush</word>
      <word>printer_close</word>
      <word>print_r</word>
      <word>print</word>
      <word>prev</word>
      <word>preg_split</word>
      <word>preg_replace</word>
      <word>preg_quote</word>
      <word>preg_match_all</word>
      <word>preg_match</word>
      <word>preg_grep</word>
      <word>pow</word>
      <word>posix_uname</word>
      <word>posix_ttyname</word>
      <word>posix_times</word>
      <word>posix_setuid</word>
      <word>posix_setsid</word>
      <word>posix_setpgid</word>
      <word>posix_setgid</word>
      <word>posix_seteuid</word>
      <word>posix_setegid</word>
      <word>posix_mkfifo</word>
      <word>posix_kill</word>
      <word>posix_isatty</word>
      <word>posix_getuid</word>
      <word>posix_getsid</word>
      <word>posix_getrlimit</word>
      <word>posix_getpwuid</word>
      <word>posix_getpwnam</word>
      <word>posix_getppid</word>
      <word>posix_getpid</word>
      <word>posix_getpgrp</word>
      <word>posix_getpgid</word>
      <word>posix_getlogin</word>
      <word>posix_getgroups</word>
      <word>posix_getgrnam</word>
      <word>posix_getgrgid</word>
      <word>posix_getgid</word>
      <word>posix_geteuid</word>
      <word>posix_getegid</word>
      <word>posix_getcwd</word>
      <word>posix_ctermid</word>
      <word>pos</word>
      <word>popen</word>
      <word>pi</word>
      <word>phpversion</word>
      <word>phpinfo</word>
      <word>phpcredits</word>
      <word>php_uname</word>
      <word>php_sapi_name</word>
      <word>php_logo_guid</word>
      <word>pg_untrace</word>
      <word>pg_tty</word>
      <word>pg_trace</word>
      <word>pg_setclientencoding</word>
      <word>pg_set_client_encoding</word>
      <word>pg_result</word>
      <word>pg_put_line</word>
      <word>pg_port</word>
      <word>pg_pconnect</word>
      <word>pg_options</word>
      <word>pg_numrows</word>
      <word>pg_numfields</word>
      <word>pg_lowrite</word>
      <word>pg_lounlink</word>
      <word>pg_loreadall</word>
      <word>pg_loread</word>
      <word>pg_loopen</word>
      <word>pg_loimport</word>
      <word>pg_loexport</word>
      <word>pg_locreate</word>
      <word>pg_loclose</word>
      <word>pg_host</word>
      <word>pg_getlastoid</word>
      <word>pg_freeresult</word>
      <word>pg_fieldtype</word>
      <word>pg_fieldsize</word>
      <word>pg_fieldprtlen</word>
      <word>pg_fieldnum</word>
      <word>pg_fieldname</word>
      <word>pg_fieldisnull</word>
      <word>pg_fetch_row</word>
      <word>pg_fetch_object</word>
      <word>pg_fetch_array</word>
      <word>pg_exec</word>
      <word>pg_errormessage</word>
      <word>pg_end_copy</word>
      <word>pg_dbname</word>
      <word>pg_connect</word>
      <word>pg_cmdtuples</word>
      <word>pg_close</word>
      <word>pg_clientencoding</word>
      <word>pg_client_encoding</word>
      <word>pfsockopen</word>
      <word>pfpro_version</word>
      <word>pfpro_process_raw</word>
      <word>pfpro_process</word>
      <word>pfpro_init</word>
      <word>pfpro_cleanup</word>
      <word>pdf_translate</word>
      <word>pdf_stroke</word>
      <word>pdf_stringwidth</word>
      <word>pdf_skew</word>
      <word>pdf_show_xy</word>
      <word>pdf_show_boxed</word>
      <word>pdf_show</word>
      <word>pdf_setrgbcolor_stroke</word>
      <word>pdf_setrgbcolor_fill</word>
      <word>pdf_setrgbcolor</word>
      <word>pdf_setmiterlimit</word>
      <word>pdf_setlinewidth</word>
      <word>pdf_setlinejoin</word>
      <word>pdf_setlinecap</word>
      <word>pdf_setgray_stroke</word>
      <word>pdf_setgray_fill</word>
      <word>pdf_setgray</word>
      <word>pdf_setflat</word>
      <word>pdf_setdash</word>
      <word>pdf_set_word_spacing</word>
      <word>pdf_set_value</word>
      <word>pdf_set_transition</word>
      <word>pdf_set_text_rise</word>
      <word>pdf_set_text_rendering</word>
      <word>pdf_set_text_pos</word>
      <word>pdf_set_parameter</word>
      <word>pdf_set_leading</word>
      <word>pdf_set_info_title</word>
      <word>pdf_set_info_subject</word>
      <word>pdf_set_info_keywords</word>
      <word>pdf_set_info_creator</word>
      <word>pdf_set_info_author</word>
      <word>pdf_set_info</word>
      <word>pdf_set_horiz_scaling</word>
      <word>pdf_set_font</word>
      <word>pdf_set_duration</word>
      <word>pdf_set_char_spacing</word>
      <word>pdf_set_border_style</word>
      <word>pdf_set_border_dash</word>
      <word>pdf_set_border_color</word>
      <word>pdf_scale</word>
      <word>pdf_save</word>
      <word>pdf_rotate</word>
      <word>pdf_restore</word>
      <word>pdf_rect</word>
      <word>pdf_place_image</word>
      <word>pdf_open_tiff</word>
      <word>pdf_open_png</word>
      <word>pdf_open_memory_image</word>
      <word>pdf_open_jpeg</word>
      <word>pdf_open_image_file</word>
      <word>pdf_open_gif</word>
      <word>pdf_open</word>
      <word>pdf_moveto</word>
      <word>pdf_lineto</word>
      <word>pdf_get_value</word>
      <word>pdf_get_parameter</word>
      <word>pdf_get_image_width</word>
      <word>pdf_get_image_height</word>
      <word>pdf_get_fontsize</word>
      <word>pdf_get_fontname</word>
      <word>pdf_get_font</word>
      <word>pdf_fill_stroke</word>
      <word>pdf_fill</word>
      <word>pdf_endpath</word>
      <word>pdf_end_page</word>
      <word>pdf_curveto</word>
      <word>pdf_continue_text</word>
      <word>pdf_closepath_stroke</word>
      <word>pdf_closepath_fill_stroke</word>
      <word>pdf_closepath</word>
      <word>pdf_close_image</word>
      <word>pdf_close</word>
      <word>pdf_clip</word>
      <word>pdf_circle</word>
      <word>pdf_begin_page</word>
      <word>pdf_arc</word>
      <word>pdf_add_weblink</word>
      <word>pdf_add_pdflink</word>
      <word>pdf_add_outline</word>
      <word>pdf_add_bookmark</word>
      <word>pdf_add_annotation</word>
      <word>pclose</word>
      <word>pathinfo</word>
      <word>passthru</word>
      <word>parse_url</word>
      <word>parse_str</word>
      <word>parse_ini_file</word>
      <word>pack</word>
      <word>ovrimos_rollback</word>
      <word>ovrimos_result_all</word>
      <word>ovrimos_result</word>
      <word>ovrimos_prepare</word>
      <word>ovrimos_num_rows</word>
      <word>ovrimos_num_fields</word>
      <word>ovrimos_longreadlen</word>
      <word>ovrimos_free_result</word>
      <word>ovrimos_field_type</word>
      <word>ovrimos_field_num</word>
      <word>ovrimos_field_name</word>
      <word>ovrimos_field_len</word>
      <word>ovrimos_fetch_row</word>
      <word>ovrimos_fetch_into</word>
      <word>ovrimos_execute</word>
      <word>ovrimos_exec</word>
      <word>ovrimos_cursor</word>
      <word>ovrimos_connect</word>
      <word>ovrimos_commit</word>
      <word>ovrimos_close_all</word>
      <word>ovrimos_close</word>
      <word>ord</word>
      <word>orbit_load_idl</word>
      <word>orbit_get_repository_id</word>
      <word>orbit_exception_value</word>
      <word>orbit_exception_id</word>
      <word>orbit_caught_exception</word>
      <word>ora_rollback</word>
      <word>ora_plogon</word>
      <word>ora_parse</word>
      <word>ora_open</word>
      <word>ora_numrows</word>
      <word>ora_numcols</word>
      <word>ora_logon</word>
      <word>ora_logoff</word>
      <word>ora_getcolumn</word>
      <word>ora_fetch_into</word>
      <word>ora_fetch</word>
      <word>ora_exec</word>
      <word>ora_errorcode</word>
      <word>ora_error</word>
      <word>ora_do</word>
      <word>ora_commiton</word>
      <word>ora_commitoff</word>
      <word>ora_commit</word>
      <word>ora_columntype</word>
      <word>ora_columnsize</word>
      <word>ora_columnname</word>
      <word>ora_close</word>
      <word>ora_bind</word>
      <word>openssl_verify</word>
      <word>openssl_sign</word>
      <word>openssl_seal</word>
      <word>openssl_read_x509</word>
      <word>openssl_read_publickey</word>
      <word>openssl_open</word>
      <word>openssl_get_publickey</word>
      <word>openssl_get_privatekey</word>
      <word>openssl_free_x509</word>
      <word>openssl_free_key</word>
      <word>openlog</word>
      <word>opendir</word>
      <word>open_listen_sock</word>
      <word>odbc_tables</word>
      <word>odbc_tableprivileges</word>
      <word>odbc_statistics</word>
      <word>odbc_specialcolumns</word>
      <word>odbc_setoption</word>
      <word>odbc_rollback</word>
      <word>odbc_result_all</word>
      <word>odbc_result</word>
      <word>odbc_procedures</word>
      <word>odbc_procedurecolumns</word>
      <word>odbc_primarykeys</word>
      <word>odbc_prepare</word>
      <word>odbc_pconnect</word>
      <word>odbc_num_rows</word>
      <word>odbc_num_fields</word>
      <word>odbc_longreadlen</word>
      <word>odbc_gettypeinfo</word>
      <word>odbc_free_result</word>
      <word>odbc_foreignkeys</word>
      <word>odbc_field_type</word>
      <word>odbc_field_scale</word>
      <word>odbc_field_precision</word>
      <word>odbc_field_num</word>
      <word>odbc_field_name</word>
      <word>odbc_field_len</word>
      <word>odbc_fetch_row</word>
      <word>odbc_fetch_object</word>
      <word>odbc_fetch_into</word>
      <word>odbc_fetch_array</word>
      <word>odbc_execute</word>
      <word>odbc_exec</word>
      <word>odbc_do</word>
      <word>odbc_cursor</word>
      <word>odbc_connect</word>
      <word>odbc_commit</word>
      <word>odbc_columns</word>
      <word>odbc_columnprivileges</word>
      <word>odbc_close_all</word>
      <word>odbc_close</word>
      <word>odbc_binmode</word>
      <word>odbc_autocommit</word>
      <word>octdec</word>
      <word>ociwritelobtofile</word>
      <word>ocistatementtype</word>
      <word>ocisetprefetch</word>
      <word>ociserverversion</word>
      <word>ocisavelobfile</word>
      <word>ocisavelob</word>
      <word>ocirowcount</word>
      <word>ocirollback</word>
      <word>ociresult</word>
      <word>ociplogon</word>
      <word>ociparse</word>
      <word>ocinumcols</word>
      <word>ocinlogon</word>
      <word>ocinewdescriptor</word>
      <word>ocinewcursor</word>
      <word>ocilogon</word>
      <word>ocilogoff</word>
      <word>ociloadlob</word>
      <word>ociinternaldebug</word>
      <word>ocifreestatement</word>
      <word>ocifreedesc</word>
      <word>ocifreecursor</word>
      <word>ocifetchstatement</word>
      <word>ocifetchinto</word>
      <word>ocifetch</word>
      <word>ociexecute</word>
      <word>ocierror</word>
      <word>ocidefinebyname</word>
      <word>ocicommit</word>
      <word>ocicolumntyperaw</word>
      <word>ocicolumntype</word>
      <word>ocicolumnsize</word>
      <word>ocicolumnscale</word>
      <word>ocicolumnprecision</word>
      <word>ocicolumnname</word>
      <word>ocicolumnisnull</word>
      <word>ocicancel</word>
      <word>ocibindbyname</word>
      <word>ob_start</word>
      <word>ob_implicit_flush</word>
      <word>ob_gzhandler</word>
      <word>ob_get_length</word>
      <word>ob_get_contents</word>
      <word>ob_end_flush</word>
      <word>ob_end_clean</word>
      <word>number_format</word>
      <word>nl2br</word>
      <word>new_xmldoc</word>
      <word>natsort</word>
      <word>natcasesort</word>
      <word>mysql_tablename</word>
      <word>mysql_selectdb</word>
      <word>mysql_select_db</word>
      <word>mysql_result</word>
      <word>mysql_query</word>
      <word>mysql_pconnect</word>
      <word>mysql_numrows</word>
      <word>mysql_numfields</word>
      <word>mysql_num_rows</word>
      <word>mysql_num_fields</word>
      <word>mysql_listtables</word>
      <word>mysql_listfields</word>
      <word>mysql_listdbs</word>
      <word>mysql_list_tables</word>
      <word>mysql_list_fields</word>
      <word>mysql_list_dbs</word>
      <word>mysql_insert_id</word>
      <word>mysql_freeresult</word>
      <word>mysql_free_result</word>
      <word>mysql_fieldtype</word>
      <word>mysql_fieldtable</word>
      <word>mysql_fieldname</word>
      <word>mysql_fieldlen</word>
      <word>mysql_fieldflags</word>
      <word>mysql_field_type</word>
      <word>mysql_field_table</word>
      <word>mysql_field_seek</word>
      <word>mysql_field_name</word>
      <word>mysql_field_len</word>
      <word>mysql_field_flags</word>
      <word>mysql_fetch_row</word>
      <word>mysql_fetch_object</word>
      <word>mysql_fetch_lengths</word>
      <word>mysql_fetch_field</word>
      <word>mysql_fetch_assoc</word>
      <word>mysql_fetch_array</word>
      <word>mysql_escape_string</word>
      <word>mysql_error</word>
      <word>mysql_errno</word>
      <word>mysql_dropdb</word>
      <word>mysql_drop_db</word>
      <word>mysql_dbname</word>
      <word>mysql_db_query</word>
      <word>mysql_db_name</word>
      <word>mysql_data_seek</word>
      <word>mysql_createdb</word>
      <word>mysql_create_db</word>
      <word>mysql_connect</word>
      <word>mysql_close</word>
      <word>mysql_affected_rows</word>
      <word>mysql</word>
      <word>mt_srand</word>
      <word>mt_rand</word>
      <word>mt_getrandmax</word>
      <word>mssql_select_db</word>
      <word>mssql_rows_affected</word>
      <word>mssql_result</word>
      <word>mssql_query</word>
      <word>mssql_pconnect</word>
      <word>mssql_num_rows</word>
      <word>mssql_num_fields</word>
      <word>mssql_min_server_severity</word>
      <word>mssql_min_message_severity</word>
      <word>mssql_min_error_severity</word>
      <word>mssql_min_client_severity</word>
      <word>mssql_get_last_message</word>
      <word>mssql_free_result</word>
      <word>mssql_field_type</word>
      <word>mssql_field_seek</word>
      <word>mssql_field_name</word>
      <word>mssql_field_length</word>
      <word>mssql_fetch_row</word>
      <word>mssql_fetch_object</word>
      <word>mssql_fetch_field</word>
      <word>mssql_fetch_batch</word>
      <word>mssql_fetch_array</word>
      <word>mssql_data_seek</word>
      <word>mssql_connect</word>
      <word>mssql_close</word>
      <word>mssql_affected_rows</word>
      <word>msql_tablename</word>
      <word>msql_selectdb</word>
      <word>msql_select_db</word>
      <word>msql_result</word>
      <word>msql_regcase</word>
      <word>msql_query</word>
      <word>msql_pconnect</word>
      <word>msql_numrows</word>
      <word>msql_numfields</word>
      <word>msql_num_rows</word>
      <word>msql_num_fields</word>
      <word>msql_listtables</word>
      <word>msql_listfields</word>
      <word>msql_listdbs</word>
      <word>msql_list_tables</word>
      <word>msql_list_fields</word>
      <word>msql_list_dbs</word>
      <word>msql_freeresult</word>
      <word>msql_free_result</word>
      <word>msql_fieldtype</word>
      <word>msql_fieldtable</word>
      <word>msql_fieldname</word>
      <word>msql_fieldlen</word>
      <word>msql_fieldflags</word>
      <word>msql_field_type</word>
      <word>msql_field_table</word>
      <word>msql_field_seek</word>
      <word>msql_field_name</word>
      <word>msql_field_len</word>
      <word>msql_field_flags</word>
      <word>msql_fetch_row</word>
      <word>msql_fetch_object</word>
      <word>msql_fetch_field</word>
      <word>msql_fetch_array</word>
      <word>msql_error</word>
      <word>msql_dropdb</word>
      <word>msql_drop_db</word>
      <word>msql_dbname</word>
      <word>msql_db_query</word>
      <word>msql_data_seek</word>
      <word>msql_createdb</word>
      <word>msql_create_db</word>
      <word>msql_connect</word>
      <word>msql_close</word>
      <word>msql_affected_rows</word>
      <word>msql</word>
      <word>move_uploaded_file</word>
      <word>mktime</word>
      <word>mkdir</word>
      <word>min</word>
      <word>microtime</word>
      <word>mhash_keygen_s2k</word>
      <word>mhash_get_hash_name</word>
      <word>mhash_get_block_size</word>
      <word>mhash_count</word>
      <word>mhash</word>
      <word>method_exists</word>
      <word>metaphone</word>
      <word>mdecrypt_generic</word>
      <word>md5</word>
      <word>mcrypt_ofb</word>
      <word>mcrypt_module_self_test</word>
      <word>mcrypt_module_open</word>
      <word>mcrypt_module_is_block_mode</word>
      <word>mcrypt_module_is_block_algorithm_mode</word>
      <word>mcrypt_module_is_block_algorithm</word>
      <word>mcrypt_module_get_supported_key_sizes</word>
      <word>mcrypt_module_get_algo_key_size</word>
      <word>mcrypt_module_get_algo_block_size</word>
      <word>mcrypt_module_close</word>
      <word>mcrypt_list_modes</word>
      <word>mcrypt_list_algorithms</word>
      <word>mcrypt_get_key_size</word>
      <word>mcrypt_get_iv_size</word>
      <word>mcrypt_get_cipher_name</word>
      <word>mcrypt_get_block_size</word>
      <word>mcrypt_generic_init</word>
      <word>mcrypt_generic_end</word>
      <word>mcrypt_generic</word>
      <word>mcrypt_encrypt</word>
      <word>mcrypt_enc_self_test</word>
      <word>mcrypt_enc_is_block_mode</word>
      <word>mcrypt_enc_is_block_algorithm_mode</word>
      <word>mcrypt_enc_is_block_algorithm</word>
      <word>mcrypt_enc_get_supported_key_sizes</word>
      <word>mcrypt_enc_get_modes_name</word>
      <word>mcrypt_enc_get_key_size</word>
      <word>mcrypt_enc_get_iv_size</word>
      <word>mcrypt_enc_get_block_size</word>
      <word>mcrypt_enc_get_algorithms_name</word>
      <word>mcrypt_ecb</word>
      <word>mcrypt_decrypt</word>
      <word>mcrypt_create_iv</word>
      <word>mcrypt_cfb</word>
      <word>mcrypt_cbc</word>
      <word>mcal_week_of_year</word>
      <word>mcal_time_valid</word>
      <word>mcal_store_event</word>
      <word>mcal_snooze</word>
      <word>mcal_reopen</word>
      <word>mcal_rename_calendar</word>
      <word>mcal_popen</word>
      <word>mcal_open</word>
      <word>mcal_next_recurrence</word>
      <word>mcal_list_events</word>
      <word>mcal_list_alarms</word>
      <word>mcal_is_leap_year</word>
      <word>mcal_fetch_event</word>
      <word>mcal_fetch_current_stream_event</word>
      <word>mcal_event_set_title</word>
      <word>mcal_event_set_start</word>
      <word>mcal_event_set_recur_yearly</word>
      <word>mcal_event_set_recur_weekly</word>
      <word>mcal_event_set_recur_none</word>
      <word>mcal_event_set_recur_monthly_wday</word>
      <word>mcal_event_set_recur_monthly_mday</word>
      <word>mcal_event_set_recur_daily</word>
      <word>mcal_event_set_end</word>
      <word>mcal_event_set_description</word>
      <word>mcal_event_set_class</word>
      <word>mcal_event_set_category</word>
      <word>mcal_event_set_alarm</word>
      <word>mcal_event_init</word>
      <word>mcal_event_add_attribute</word>
      <word>mcal_delete_event</word>
      <word>mcal_delete_calendar</word>
      <word>mcal_days_in_month</word>
      <word>mcal_day_of_year</word>
      <word>mcal_day_of_week</word>
      <word>mcal_date_valid</word>
      <word>mcal_date_compare</word>
      <word>mcal_create_calendar</word>
      <word>mcal_close</word>
      <word>mcal_append_event</word>
      <word>max</word>
      <word>mail</word>
      <word>magic_quotes_runtime</word>
      <word>ltrim</word>
      <word>lstat</word>
      <word>long2ip</word>
      <word>log10</word>
      <word>log</word>
      <word>localtime</word>
      <word>listen</word>
      <word>list</word>
      <word>linkinfo</word>
      <word>link</word>
      <word>levenshtein</word>
      <word>leak</word>
      <word>ldap_unbind</word>
      <word>ldap_t61_to_8859</word>
      <word>ldap_set_option</word>
      <word>ldap_search</word>
      <word>ldap_read</word>
      <word>ldap_next_entry</word>
      <word>ldap_next_attribute</word>
      <word>ldap_modify</word>
      <word>ldap_mod_replace</word>
      <word>ldap_mod_del</word>
      <word>ldap_mod_add</word>
      <word>ldap_list</word>
      <word>ldap_get_values_len</word>
      <word>ldap_get_values</word>
      <word>ldap_get_option</word>
      <word>ldap_get_entries</word>
      <word>ldap_get_dn</word>
      <word>ldap_get_attributes</word>
      <word>ldap_free_result</word>
      <word>ldap_first_entry</word>
      <word>ldap_first_attribute</word>
      <word>ldap_explode_dn</word>
      <word>ldap_error</word>
      <word>ldap_errno</word>
      <word>ldap_err2str</word>
      <word>ldap_dn2ufn</word>
      <word>ldap_delete</word>
      <word>ldap_count_entries</word>
      <word>ldap_connect</word>
      <word>ldap_compare</word>
      <word>ldap_close</word>
      <word>ldap_bind</word>
      <word>ldap_add</word>
      <word>ldap_8859_to_t61</word>
      <word>lcg_value</word>
      <word>ksort</word>
      <word>krsort</word>
      <word>key</word>
      <word>juliantojd</word>
      <word>join</word>
      <word>jewishtojd</word>
      <word>jdtounix</word>
      <word>jdtojulian</word>
      <word>jdtojewish</word>
      <word>jdtogregorian</word>
      <word>jdtofrench</word>
      <word>jdmonthname</word>
      <word>jddayofweek</word>
      <word>java_last_exception_get</word>
      <word>java_last_exception_clear</word>
      <word>isxdigit</word>
      <word>isupper</word>
      <word>isspace</word>
      <word>ispunct</word>
      <word>isprint</word>
      <word>islower</word>
      <word>isgraph</word>
      <word>isdigit</word>
      <word>iscntrl</word>
      <word>isalpha</word>
      <word>isalnum</word>
      <word>is_writeable</word>
      <word>is_writable</word>
      <word>is_uploaded_file</word>
      <word>is_subclass_of</word>
      <word>is_string</word>
      <word>is_resource</word>
      <word>is_real</word>
      <word>is_readable</word>
      <word>is_object</word>
      <word>is_numeric</word>
      <word>is_null</word>
      <word>is_long</word>
      <word>is_link</word>
      <word>is_integer</word>
      <word>is_int</word>
      <word>is_float</word>
      <word>is_file</word>
      <word>is_executable</word>
      <word>is_double</word>
      <word>is_dir</word>
      <word>is_bool</word>
      <word>is_array</word>
      <word>ircg_set_current</word>
      <word>ircg_pconnect</word>
      <word>ircg_part</word>
      <word>ircg_msg</word>
      <word>ircg_join</word>
      <word>ircg_disconnect</word>
      <word>iptcparse</word>
      <word>iptcembed</word>
      <word>ip2long</word>
      <word>intval</word>
      <word>ini_set</word>
      <word>ini_restore</word>
      <word>ini_get</word>
      <word>ini_alter</word>
      <word>ingres_rollback</word>
      <word>ingres_query</word>
      <word>ingres_pconnect</word>
      <word>ingres_num_rows</word>
      <word>ingres_num_fields</word>
      <word>ingres_field_type</word>
      <word>ingres_field_scale</word>
      <word>ingres_field_precision</word>
      <word>ingres_field_nullable</word>
      <word>ingres_field_name</word>
      <word>ingres_field_length</word>
      <word>ingres_fetch_row</word>
      <word>ingres_fetch_object</word>
      <word>ingres_fetch_array</word>
      <word>ingres_connect</word>
      <word>ingres_commit</word>
      <word>ingres_close</word>
      <word>ingres_autocommit</word>
      <word>in_array</word>
      <word>implode</word>
      <word>imap_utf8</word>
      <word>imap_utf7_encode</word>
      <word>imap_utf7_decode</word>
      <word>imap_unsubscribe</word>
      <word>imap_undelete</word>
      <word>imap_uid</word>
      <word>imap_subscribe</word>
      <word>imap_status</word>
      <word>imap_sort</word>
      <word>imap_setflag_full</word>
      <word>imap_search</word>
      <word>imap_scanmailbox</word>
      <word>imap_scan</word>
      <word>imap_rfc822_write_address</word>
      <word>imap_rfc822_parse_headers</word>
      <word>imap_rfc822_parse_adrlist</word>
      <word>imap_reopen</word>
      <word>imap_renamemailbox</word>
      <word>imap_rename</word>
      <word>imap_qprint</word>
      <word>imap_popen</word>
      <word>imap_ping</word>
      <word>imap_open</word>
      <word>imap_num_recent</word>
      <word>imap_num_msg</word>
      <word>imap_msgno</word>
      <word>imap_mime_header_decode</word>
      <word>imap_mailboxmsginfo</word>
      <word>imap_mail_move</word>
      <word>imap_mail_copy</word>
      <word>imap_mail_compose</word>
      <word>imap_mail</word>
      <word>imap_lsub</word>
      <word>imap_listsubscribed</word>
      <word>imap_listmailbox</word>
      <word>imap_list</word>
      <word>imap_last_error</word>
      <word>imap_headers</word>
      <word>imap_headerinfo</word>
      <word>imap_header</word>
      <word>imap_getsubscribed</word>
      <word>imap_getmailboxes</word>
      <word>imap_fetchtext</word>
      <word>imap_fetchstructure</word>
      <word>imap_fetchheader</word>
      <word>imap_fetchbody</word>
      <word>imap_fetch_overview</word>
      <word>imap_expunge</word>
      <word>imap_errors</word>
      <word>imap_deletemailbox</word>
      <word>imap_delete</word>
      <word>imap_createmailbox</word>
      <word>imap_create</word>
      <word>imap_close</word>
      <word>imap_clearflag_full</word>
      <word>imap_check</word>
      <word>imap_bodystruct</word>
      <word>imap_body</word>
      <word>imap_binary</word>
      <word>imap_base64</word>
      <word>imap_append</word>
      <word>imap_alerts</word>
      <word>imap_8bit</word>
      <word>imagewbmp</word>
      <word>imagetypes</word>
      <word>imagettftext</word>
      <word>imagettfbbox</word>
      <word>imagesy</word>
      <word>imagesx</word>
      <word>imagestringup</word>
      <word>imagestring</word>
      <word>imagesetpixel</word>
      <word>imagerectangle</word>
      <word>imagepstext</word>
      <word>imagepsslantfont</word>
      <word>imagepsloadfont</word>
      <word>imagepsfreefont</word>
      <word>imagepsextendfont</word>
      <word>imagepsencodefont</word>
      <word>imagepscopyfont</word>
      <word>imagepsbbox</word>
      <word>imagepolygon</word>
      <word>imagepng</word>
      <word>imagepalettecopy</word>
      <word>imageloadfont</word>
      <word>imageline</word>
      <word>imagejpeg</word>
      <word>imageinterlace</word>
      <word>imagegif</word>
      <word>imagegammacorrect</word>
      <word>imagefontwidth</word>
      <word>imagefontheight</word>
      <word>imagefilltoborder</word>
      <word>imagefilledrectangle</word>
      <word>imagefilledpolygon</word>
      <word>imagefill</word>
      <word>imagedestroy</word>
      <word>imagedashedline</word>
      <word>imagecreatefromxpm</word>
      <word>imagecreatefromxbm</word>
      <word>imagecreatefromwbmp</word>
      <word>imagecreatefromstring</word>
      <word>imagecreatefrompng</word>
      <word>imagecreatefromjpeg</word>
      <word>imagecreatefromgif</word>
      <word>imagecreate</word>
      <word>imagecopyresized</word>
      <word>imagecopymerge</word>
      <word>imagecopy</word>
      <word>imagecolortransparent</word>
      <word>imagecolorstotal</word>
      <word>imagecolorsforindex</word>
      <word>imagecolorset</word>
      <word>imagecolorresolve</word>
      <word>imagecolorexact</word>
      <word>imagecolordeallocate</word>
      <word>imagecolorclosesthwb</word>
      <word>imagecolorclosest</word>
      <word>imagecolorat</word>
      <word>imagecolorallocate</word>
      <word>imagecharup</word>
      <word>imagechar</word>
      <word>imagearc</word>
      <word>iis_stopservice</word>
      <word>iis_stopserver</word>
      <word>iis_startservice</word>
      <word>iis_startserver</word>
      <word>iis_setserverright</word>
      <word>iis_setscriptmap</word>
      <word>iis_setdirsecurity</word>
      <word>iis_setappsettings</word>
      <word>iis_removeserver</word>
      <word>iis_getservicestate</word>
      <word>iis_getserverright</word>
      <word>iis_getserverbypath</word>
      <word>iis_getserverbycomment</word>
      <word>iis_getscriptmap</word>
      <word>iis_getdirsecurity</word>
      <word>iis_addserver</word>
      <word>ignore_user_abort</word>
      <word>ifxus_write_slob</word>
      <word>ifxus_tell_slob</word>
      <word>ifxus_seek_slob</word>
      <word>ifxus_read_slob</word>
      <word>ifxus_open_slob</word>
      <word>ifxus_free_slob</word>
      <word>ifxus_create_slob</word>
      <word>ifxus_close_slob</word>
      <word>ifx_update_char</word>
      <word>ifx_update_blob</word>
      <word>ifx_textasvarchar</word>
      <word>ifx_query</word>
      <word>ifx_prepare</word>
      <word>ifx_pconnect</word>
      <word>ifx_num_rows</word>
      <word>ifx_num_fields</word>
      <word>ifx_nullformat</word>
      <word>ifx_htmltbl_result</word>
      <word>ifx_getsqlca</word>
      <word>ifx_get_char</word>
      <word>ifx_get_blob</word>
      <word>ifx_free_result</word>
      <word>ifx_free_char</word>
      <word>ifx_free_blob</word>
      <word>ifx_fieldtypes</word>
      <word>ifx_fieldproperties</word>
      <word>ifx_fetch_row</word>
      <word>ifx_errormsg</word>
      <word>ifx_error</word>
      <word>ifx_do</word>
      <word>ifx_create_char</word>
      <word>ifx_create_blob</word>
      <word>ifx_copy_blob</word>
      <word>ifx_connect</word>
      <word>ifx_close</word>
      <word>ifx_byteasvarchar</word>
      <word>ifx_blobinfile_mode</word>
      <word>ifx_affected_rows</word>
      <word>icap_store_event</word>
      <word>icap_snooze</word>
      <word>icap_reopen</word>
      <word>icap_rename_calendar</word>
      <word>icap_popen</word>
      <word>icap_open</word>
      <word>icap_list_events</word>
      <word>icap_list_alarms</word>
      <word>icap_fetch_event</word>
      <word>icap_delete_event</word>
      <word>icap_delete_calendar</word>
      <word>icap_create_calendar</word>
      <word>ibase_trans</word>
      <word>ibase_timefmt</word>
      <word>ibase_rollback</word>
      <word>ibase_query</word>
      <word>ibase_prepare</word>
      <word>ibase_pconnect</word>
      <word>ibase_num_fields</word>
      <word>ibase_free_result</word>
      <word>ibase_free_query</word>
      <word>ibase_field_info</word>
      <word>ibase_fetch_row</word>
      <word>ibase_fetch_object</word>
      <word>ibase_execute</word>
      <word>ibase_errmsg</word>
      <word>ibase_connect</word>
      <word>ibase_commit</word>
      <word>ibase_close</word>
      <word>ibase_blob_open</word>
      <word>ibase_blob_info</word>
      <word>ibase_blob_import</word>
      <word>ibase_blob_get</word>
      <word>ibase_blob_echo</word>
      <word>ibase_blob_create</word>
      <word>ibase_blob_close</word>
      <word>ibase_blob_cancel</word>
      <word>ibase_blob_add</word>
      <word>hw_who</word>
      <word>hw_unlock</word>
      <word>hw_stat</word>
      <word>hw_setlinkroot</word>
      <word>hw_root</word>
      <word>hw_pipedocument</word>
      <word>hw_pipecgi</word>
      <word>hw_pconnect</word>
      <word>hw_output_document</word>
      <word>hw_objrec2array</word>
      <word>hw_new_document</word>
      <word>hw_mv</word>
      <word>hw_modifyobject</word>
      <word>hw_mapid</word>
      <word>hw_insertobject</word>
      <word>hw_insertdocument</word>
      <word>hw_insertanchors</word>
      <word>hw_insdoc</word>
      <word>hw_inscoll</word>
      <word>hw_info</word>
      <word>hw_incollections</word>
      <word>hw_identify</word>
      <word>hw_getusername</word>
      <word>hw_gettext</word>
      <word>hw_getsrcbydestobj</word>
      <word>hw_getremotechildren</word>
      <word>hw_getremote</word>
      <word>hw_getrellink</word>
      <word>hw_getparentsobj</word>
      <word>hw_getparents</word>
      <word>hw_getobjectbyqueryobj</word>
      <word>hw_getobjectbyquerycollobj</word>
      <word>hw_getobjectbyquerycoll</word>
      <word>hw_getobjectbyquery</word>
      <word>hw_getobjectbyftqueryobj</word>
      <word>hw_getobjectbyftquerycollobj</word>
      <word>hw_getobjectbyftquerycoll</word>
      <word>hw_getobjectbyftquery</word>
      <word>hw_getobject</word>
      <word>hw_getchilddoccollobj</word>
      <word>hw_getchilddoccoll</word>
      <word>hw_getchildcollobj</word>
      <word>hw_getchildcoll</word>
      <word>hw_getcgi</word>
      <word>hw_getandlock</word>
      <word>hw_getanchorsobj</word>
      <word>hw_getanchors</word>
      <word>hw_free_document</word>
      <word>hw_errormsg</word>
      <word>hw_error</word>
      <word>hw_edittext</word>
      <word>hw_dummy</word>
      <word>hw_document_size</word>
      <word>hw_document_setcontent</word>
      <word>hw_document_content</word>
      <word>hw_document_bodytag</word>
      <word>hw_document_attributes</word>
      <word>hw_docbyanchorobj</word>
      <word>hw_docbyanchor</word>
      <word>hw_deleteobject</word>
      <word>hw_cp</word>
      <word>hw_connection_info</word>
      <word>hw_connect</word>
      <word>hw_close</word>
      <word>hw_childrenobj</word>
      <word>hw_children</word>
      <word>hw_changeobject</word>
      <word>hw_array2objrec</word>
      <word>htmlspecialchars</word>
      <word>htmlentities</word>
      <word>highlight_string</word>
      <word>highlight_file</word>
      <word>hexdec</word>
      <word>hebrevc</word>
      <word>hebrev</word>
      <word>headers_sent</word>
      <word>header</word>
      <word>gzwrite</word>
      <word>gzuncompress</word>
      <word>gztell</word>
      <word>gzseek</word>
      <word>gzrewind</word>
      <word>gzread</word>
      <word>gzputs</word>
      <word>gzpassthru</word>
      <word>gzopen</word>
      <word>gzinflate</word>
      <word>gzgetss</word>
      <word>gzgets</word>
      <word>gzgetc</word>
      <word>gzfile</word>
      <word>gzeof</word>
      <word>gzencode</word>
      <word>gzdeflate</word>
      <word>gzcompress</word>
      <word>gzclose</word>
      <word>gregoriantojd</word>
      <word>gmstrftime</word>
      <word>gmp_xor</word>
      <word>gmp_sub</word>
      <word>gmp_strval</word>
      <word>gmp_sqrtrem</word>
      <word>gmp_sqrt</word>
      <word>gmp_sign</word>
      <word>gmp_setbit</word>
      <word>gmp_scan1</word>
      <word>gmp_scan0</word>
      <word>gmp_random</word>
      <word>gmp_prob_prime</word>
      <word>gmp_powm</word>
      <word>gmp_pow</word>
      <word>gmp_popcount</word>
      <word>gmp_perfect_square</word>
      <word>gmp_or</word>
      <word>gmp_neg</word>
      <word>gmp_mul</word>
      <word>gmp_mod</word>
      <word>gmp_legendre</word>
      <word>gmp_jacobi</word>
      <word>gmp_invert</word>
      <word>gmp_intval</word>
      <word>gmp_init</word>
      <word>gmp_hamdist</word>
      <word>gmp_gcdext</word>
      <word>gmp_gcd</word>
      <word>gmp_fact</word>
      <word>gmp_divexact</word>
      <word>gmp_div_r</word>
      <word>gmp_div_qr</word>
      <word>gmp_div_q</word>
      <word>gmp_com</word>
      <word>gmp_cmp</word>
      <word>gmp_clrbit</word>
      <word>gmp_and</word>
      <word>gmp_add</word>
      <word>gmp_abs</word>
      <word>gmmktime</word>
      <word>gmdate</word>
      <word>gettype</word>
      <word>gettimeofday</word>
      <word>gettext</word>
      <word>getsockopt</word>
      <word>getsockname</word>
      <word>getservbyport</word>
      <word>getservbyname</word>
      <word>getrusage</word>
      <word>getrandmax</word>
      <word>getprotobynumber</word>
      <word>getprotobyname</word>
      <word>getpeername</word>
      <word>getmyuid</word>
      <word>getmypid</word>
      <word>getmyinode</word>
      <word>getmxrr</word>
      <word>getlastmod</word>
      <word>getimagesize</word>
      <word>gethostbynamel</word>
      <word>gethostbyname</word>
      <word>gethostbyaddr</word>
      <word>getenv</word>
      <word>getdate</word>
      <word>getcwd</word>
      <word>getallheaders</word>
      <word>get_resource_type</word>
      <word>get_parent_class</word>
      <word>get_object_vars</word>
      <word>get_meta_tags</word>
      <word>get_magic_quotes_runtime</word>
      <word>get_magic_quotes_gpc</word>
      <word>get_loaded_extensions</word>
      <word>get_included_files</word>
      <word>get_html_translation_table</word>
      <word>get_extension_funcs</word>
      <word>get_defined_vars</word>
      <word>get_defined_functions</word>
      <word>get_declared_classes</word>
      <word>get_current_user</word>
      <word>get_class_vars</word>
      <word>get_class_methods</word>
      <word>get_class</word>
      <word>get_cfg_var</word>
      <word>get_browser</word>
      <word>get_all_headers</word>
      <word>fwrite</word>
      <word>function_exists</word>
      <word>func_num_args</word>
      <word>func_get_args</word>
      <word>func_get_arg</word>
      <word>ftruncate</word>
      <word>ftp_systype</word>
      <word>ftp_size</word>
      <word>ftp_site</word>
      <word>ftp_rmdir</word>
      <word>ftp_rename</word>
      <word>ftp_rawlist</word>
      <word>ftp_quit</word>
      <word>ftp_pwd</word>
      <word>ftp_put</word>
      <word>ftp_pasv</word>
      <word>ftp_nlist</word>
      <word>ftp_mkdir</word>
      <word>ftp_mdtm</word>
      <word>ftp_login</word>
      <word>ftp_get</word>
      <word>ftp_fput</word>
      <word>ftp_fget</word>
      <word>ftp_exec</word>
      <word>ftp_delete</word>
      <word>ftp_connect</word>
      <word>ftp_chdir</word>
      <word>ftp_cdup</word>
      <word>ftell</word>
      <word>fstat</word>
      <word>fsockopen</word>
      <word>fseek</word>
      <word>fscanf</word>
      <word>fribidi_log2vis</word>
      <word>frenchtojd</word>
      <word>free_iovec</word>
      <word>fread</word>
      <word>fputs</word>
      <word>fpassthru</word>
      <word>fopen</word>
      <word>flush</word>
      <word>floor</word>
      <word>flock</word>
      <word>filetype</word>
      <word>filesize</word>
      <word>filepro_rowcount</word>
      <word>filepro_retrieve</word>
      <word>filepro_fieldwidth</word>
      <word>filepro_fieldtype</word>
      <word>filepro_fieldname</word>
      <word>filepro_fieldcount</word>
      <word>filepro</word>
      <word>fileperms</word>
      <word>fileowner</word>
      <word>filemtime</word>
      <word>fileinode</word>
      <word>filegroup</word>
      <word>filectime</word>
      <word>fileatime</word>
      <word>file_exists</word>
      <word>file</word>
      <word>fgetss</word>
      <word>fgets</word>
      <word>fgetcsv</word>
      <word>fgetc</word>
      <word>fflush</word>
      <word>fetch_iovec</word>
      <word>feof</word>
      <word>fdf_set_value</word>
      <word>fdf_set_submit_form_action</word>
      <word>fdf_set_status</word>
      <word>fdf_set_opt</word>
      <word>fdf_set_javascript_action</word>
      <word>fdf_set_flags</word>
      <word>fdf_set_file</word>
      <word>fdf_set_ap</word>
      <word>fdf_save</word>
      <word>fdf_open</word>
      <word>fdf_next_field_name</word>
      <word>fdf_get_value</word>
      <word>fdf_get_status</word>
      <word>fdf_get_file</word>
      <word>fdf_create</word>
      <word>fdf_close</word>
      <word>fdf_add_template</word>
      <word>fd_zero</word>
      <word>fd_set</word>
      <word>fd_isset</word>
      <word>fd_dealloc</word>
      <word>fd_clear</word>
      <word>fd_alloc</word>
      <word>fclose</word>
      <word>ezmlm_hash</word>
      <word>extract</word>
      <word>extension_loaded</word>
      <word>explode</word>
      <word>exp</word>
      <word>exit</word>
      <word>exec</word>
      <word>escapeshellcmd</word>
      <word>escapeshellarg</word>
      <word>error_reporting</word>
      <word>error_log</word>
      <word>eregi_replace</word>
      <word>eregi</word>
      <word>ereg_replace</word>
      <word>ereg</word>
      <word>end</word>
      <word>easter_days</word>
      <word>easter_date</word>
      <word>each</word>
      <word>doubleval</word>
      <word>domxml_setattr</word>
      <word>domxml_set_content</word>
      <word>domxml_root</word>
      <word>domxml_node</word>
      <word>domxml_new_xmldoc</word>
      <word>domxml_new_child</word>
      <word>domxml_getattr</word>
      <word>domxml_dumpmem</word>
      <word>domxml_children</word>
      <word>domxml_attributes</word>
      <word>domxml_add_root</word>
      <word>dl</word>
      <word>display_disabled_function</word>
      <word>diskfreespace</word>
      <word>dirname</word>
      <word>dir</word>
      <word>dgettext</word>
      <word>delete_iovec</word>
      <word>deg2rad</word>
      <word>defined</word>
      <word>define_syslog_variables</word>
      <word>define</word>
      <word>decoct</word>
      <word>dechex</word>
      <word>decbin</word>
      <word>dcgettext</word>
      <word>dbmreplace</word>
      <word>dbmopen</word>
      <word>dbmnextkey</word>
      <word>dbminsert</word>
      <word>dbmfirstkey</word>
      <word>dbmfetch</word>
      <word>dbmexists</word>
      <word>dbmdelete</word>
      <word>dbmclose</word>
      <word>dblist</word>
      <word>dbase_replace_record</word>
      <word>dbase_pack</word>
      <word>dbase_open</word>
      <word>dbase_numrecords</word>
      <word>dbase_numfields</word>
      <word>dbase_get_record_with_names</word>
      <word>dbase_get_record</word>
      <word>dbase_delete_record</word>
      <word>dbase_create</word>
      <word>dbase_close</word>
      <word>dbase_add_record</word>
      <word>dba_sync</word>
      <word>dba_replace</word>
      <word>dba_popen</word>
      <word>dba_optimize</word>
      <word>dba_open</word>
      <word>dba_nextkey</word>
      <word>dba_insert</word>
      <word>dba_firstkey</word>
      <word>dba_fetch</word>
      <word>dba_exists</word>
      <word>dba_delete</word>
      <word>dba_close</word>
      <word>dav_set_mkcol_handlers</word>
      <word>date</word>
      <word>cybercash_encr</word>
      <word>cybercash_decr</word>
      <word>cybercash_base64_encode</word>
      <word>cybercash_base64_decode</word>
      <word>cv_void</word>
      <word>cv_textvalue</word>
      <word>cv_status</word>
      <word>cv_sale</word>
      <word>cv_reverse</word>
      <word>cv_return</word>
      <word>cv_report</word>
      <word>cv_new</word>
      <word>cv_lookup</word>
      <word>cv_init</word>
      <word>cv_done</word>
      <word>cv_delete</word>
      <word>cv_count</word>
      <word>cv_command</word>
      <word>cv_auth</word>
      <word>cv_add</word>
      <word>current</word>
      <word>curl_version</word>
      <word>curl_setopt</word>
      <word>curl_init</word>
      <word>curl_getinfo</word>
      <word>curl_exec</word>
      <word>curl_error</word>
      <word>curl_errno</word>
      <word>curl_close</word>
      <word>crypt</word>
      <word>create_function</word>
      <word>crc32</word>
      <word>crash</word>
      <word>cpdf_translate</word>
      <word>cpdf_text</word>
      <word>cpdf_stroke</word>
      <word>cpdf_stringwidth</word>
      <word>cpdf_show_xy</word>
      <word>cpdf_show</word>
      <word>cpdf_setrgbcolor_stroke</word>
      <word>cpdf_setrgbcolor_fill</word>
      <word>cpdf_setrgbcolor</word>
      <word>cpdf_setmiterlimit</word>
      <word>cpdf_setlinewidth</word>
      <word>cpdf_setlinejoin</word>
      <word>cpdf_setlinecap</word>
      <word>cpdf_setgray_stroke</word>
      <word>cpdf_setgray_fill</word>
      <word>cpdf_setgray</word>
      <word>cpdf_setflat</word>
      <word>cpdf_setdash</word>
      <word>cpdf_set_word_spacing</word>
      <word>cpdf_set_viewer_preferences</word>
      <word>cpdf_set_title</word>
      <word>cpdf_set_text_rise</word>
      <word>cpdf_set_text_rendering</word>
      <word>cpdf_set_text_pos</word>
      <word>cpdf_set_text_matrix</word>
      <word>cpdf_set_subject</word>
      <word>cpdf_set_page_animation</word>
      <word>cpdf_set_leading</word>
      <word>cpdf_set_keywords</word>
      <word>cpdf_set_horiz_scaling</word>
      <word>cpdf_set_font</word>
      <word>cpdf_set_current_page</word>
      <word>cpdf_set_creator</word>
      <word>cpdf_set_char_spacing</word>
      <word>cpdf_set_action_url</word>
      <word>cpdf_scale</word>
      <word>cpdf_save_to_file</word>
      <word>cpdf_save</word>
      <word>cpdf_rotate_text</word>
      <word>cpdf_rotate</word>
      <word>cpdf_rmoveto</word>
      <word>cpdf_rlineto</word>
      <word>cpdf_restore</word>
      <word>cpdf_rect</word>
      <word>cpdf_place_inline_image</word>
      <word>cpdf_page_init</word>
      <word>cpdf_output_buffer</word>
      <word>cpdf_open</word>
      <word>cpdf_newpath</word>
      <word>cpdf_moveto</word>
      <word>cpdf_lineto</word>
      <word>cpdf_import_jpeg</word>
      <word>cpdf_global_set_document_limits</word>
      <word>cpdf_finalize_page</word>
      <word>cpdf_finalize</word>
      <word>cpdf_fill_stroke</word>
      <word>cpdf_fill</word>
      <word>cpdf_end_text</word>
      <word>cpdf_curveto</word>
      <word>cpdf_continue_text</word>
      <word>cpdf_closepath_stroke</word>
      <word>cpdf_closepath_fill_stroke</word>
      <word>cpdf_closepath</word>
      <word>cpdf_close</word>
      <word>cpdf_clip</word>
      <word>cpdf_circle</word>
      <word>cpdf_begin_text</word>
      <word>cpdf_arc</word>
      <word>cpdf_add_outline</word>
      <word>cpdf_add_annotation</word>
      <word>count_chars</word>
      <word>count</word>
      <word>cos</word>
      <word>copy</word>
      <word>convert_cyr_string</word>
      <word>constant</word>
      <word>connection_timeout</word>
      <word>connection_status</word>
      <word>connection_aborted</word>
      <word>connect</word>
      <word>confirm_zziplib_compiled</word>
      <word>confirm_extname_compiled</word>
      <word>confirm_ctype_compiled</word>
      <word>com_set</word>
      <word>com_propset</word>
      <word>com_propput</word>
      <word>com_propget</word>
      <word>com_load</word>
      <word>com_invoke</word>
      <word>com_get</word>
      <word>closelog</word>
      <word>closedir</word>
      <word>close</word>
      <word>clearstatcache</word>
      <word>class_exists</word>
      <word>chunk_split</word>
      <word>chr</word>
      <word>chown</word>
      <word>chop</word>
      <word>chmod</word>
      <word>chgrp</word>
      <word>checkdnsrr</word>
      <word>checkdate</word>
      <word>chdir</word>
      <word>ceil</word>
      <word>ccvs_void</word>
      <word>ccvs_textvalue</word>
      <word>ccvs_status</word>
      <word>ccvs_sale</word>
      <word>ccvs_reverse</word>
      <word>ccvs_return</word>
      <word>ccvs_report</word>
      <word>ccvs_new</word>
      <word>ccvs_lookup</word>
      <word>ccvs_init</word>
      <word>ccvs_done</word>
      <word>ccvs_delete</word>
      <word>ccvs_count</word>
      <word>ccvs_command</word>
      <word>ccvs_auth</word>
      <word>ccvs_add</word>
      <word>call_user_method</word>
      <word>call_user_func_array</word>
      <word>call_user_func</word>
      <word>bzwrite</word>
      <word>bzread</word>
      <word>bzopen</word>
      <word>bzflush</word>
      <word>bzerrstr</word>
      <word>bzerror</word>
      <word>bzerrno</word>
      <word>bzdecompress</word>
      <word>bzcompress</word>
      <word>bzclose</word>
      <word>build_iovec</word>
      <word>bindtextdomain</word>
      <word>bindec</word>
      <word>bind</word>
      <word>bin2hex</word>
      <word>bcsub</word>
      <word>bcsqrt</word>
      <word>bcscale</word>
      <word>bcpow</word>
      <word>bcmul</word>
      <word>bcmod</word>
      <word>bcdiv</word>
      <word>bccomp</word>
      <word>bcadd</word>
      <word>basename</word>
      <word>base64_encode</word>
      <word>base64_decode</word>
      <word>base_convert</word>
      <word>atan2</word>
      <word>atan</word>
      <word>assert_options</word>
      <word>assert</word>
      <word>aspell_suggest</word>
      <word>aspell_new</word>
      <word>aspell_check_raw</word>
      <word>aspell_check</word>
      <word>asort</word>
      <word>asin</word>
      <word>arsort</word>
      <word>array_walk</word>
      <word>array_values</word>
      <word>array_unshift</word>
      <word>array_unique</word>
      <word>array_sum</word>
      <word>array_splice</word>
      <word>array_slice</word>
      <word>array_shift</word>
      <word>array_reverse</word>
      <word>array_rand</word>
      <word>array_push</word>
      <word>array_pop</word>
      <word>array_pad</word>
      <word>array_multisort</word>
      <word>array_merge_recursive</word>
      <word>array_merge</word>
      <word>array_keys</word>
      <word>array_intersect</word>
      <word>array_flip</word>
      <word>array_diff</word>
      <word>array_count_values</word>
      <word>array</word>
      <word>apache_sub_req</word>
      <word>apache_note</word>
      <word>apache_lookup_uri</word>
      <word>addslashes</word>
      <word>addcslashes</word>
      <word>add_iovec</word>
      <word>acos</word>
      <word>accept_connect</word>
      <word>abs</word>
    </pattern>
  </definition>
  <definition name=""Python"" caseSensitive=""true"">
    <default>
      <font name=""Menlo"" size=""11"" style=""normal"" foreColor=""black"" backColor=""transparent""/>
    </default>
    <pattern name=""Comment"" type=""block"" beginsWith=""//"" endsWith=""\n"">
      <font name=""Menlo"" size=""11"" style=""normal"" foreColor=""green"" backColor=""transparent""/>
    </pattern>
    <pattern name=""MultiLineComment"" type=""block"" beginsWith=""/*"" endsWith=""*/"">
      <font name=""Menlo"" size=""11"" style=""normal"" foreColor=""green"" backColor=""transparent""/>
    </pattern>
    <pattern name=""String"" type=""block"" beginsWith=""&quot;"" endsWith=""&quot;"" escapesWith=""\"">
      <font name=""Menlo"" size=""11"" style=""normal"" foreColor=""#666666"" backColor=""transparent""/>
    </pattern>
    <pattern name=""WordGroup01"" type=""word"">
      <font name=""Menlo"" size=""11"" style=""normal"" foreColor=""blue"" backColor=""transparent""/>
      <word>zfill</word>
      <word>XRangeType</word>
      <word>xor</word>
      <word>writelines</word>
      <word>write</word>
      <word>write</word>
      <word>winver</word>
      <word>whitespace</word>
      <word>WeakValueDictionary</word>
      <word>weakref</word>
      <word>WeakKeyDictionary</word>
      <word>warnoptions</word>
      <word>warnings</word>
      <word>warn_explicit</word>
      <word>warn</word>
      <word>version_info</word>
      <word>version</word>
      <word>UserString</word>
      <word>UserList</word>
      <word>UserList</word>
      <word>UserDict</word>
      <word>UserDict</word>
      <word>user</word>
      <word>uppercase</word>
      <word>upper</word>
      <word>UnpicklingError</word>
      <word>Unpickler</word>
      <word>unpack</word>
      <word>UnicodeType</word>
      <word>UnboundMethodType</word>
      <word>TypeType</word>
      <word>types</word>
      <word>turnon_sigfpe</word>
      <word>turnoff_sigfpe</word>
      <word>TupleType</word>
      <word>truth</word>
      <word>truediv</word>
      <word>translate</word>
      <word>TracebackType</word>
      <word>tracebacklimit</word>
      <word>traceback</word>
      <word>trace</word>
      <word>time</word>
      <word>tb_lineno</word>
      <word>sys</word>
      <word>swapcase</word>
      <word>subn</word>
      <word>sub</word>
      <word>sub</word>
      <word>strip</word>
      <word>StringTypes</word>
      <word>StringType</word>
      <word>StringIO</word>
      <word>string</word>
      <word>StreamWriter</word>
      <word>StreamRecoder</word>
      <word>StreamReaderWriter</word>
      <word>StreamReader</word>
      <word>stream_writer</word>
      <word>stream_reader</word>
      <word>stdout</word>
      <word>stdin</word>
      <word>stderr</word>
      <word>start</word>
      <word>stack</word>
      <word>splitfields</word>
      <word>split</word>
      <word>split</word>
      <word>span</word>
      <word>SliceType</word>
      <word>site</word>
      <word>showwarning</word>
      <word>showtraceback</word>
      <word>showsyntaxerror</word>
      <word>shelve</word>
      <word>settrace</word>
      <word>setslice</word>
      <word>setrecursionlimit</word>
      <word>setprofile</word>
      <word>setitem</word>
      <word>setdlopenflags</word>
      <word>setdefaultencoding</word>
      <word>setcheckinterval</word>
      <word>set_threshold</word>
      <word>set_seqs</word>
      <word>set_seq2</word>
      <word>set_seq1</word>
      <word>set_debug</word>
      <word>SequenceMatcher</word>
      <word>sequenceIncludes</word>
      <word>SEARCH_ERROR</word>
      <word>search</word>
      <word>sci</word>
      <word>saferepr</word>
      <word>runsource</word>
      <word>runcode</word>
      <word>rstrip</word>
      <word>rshift</word>
      <word>rjust</word>
      <word>rindex</word>
      <word>rfind</word>
      <word>return</word>
      <word>restore</word>
      <word>resetwarnings</word>
      <word>resetbuffer</word>
      <word>reset</word>
      <word>reset</word>
      <word>repr1</word>
      <word>repr_type</word>
      <word>Repr</word>
      <word>repr</word>
      <word>repr</word>
      <word>repr</word>
      <word>replace</word>
      <word>repeat</word>
      <word>register</word>
      <word>register</word>
      <word>ReferenceType</word>
      <word>ReferenceError</word>
      <word>ref</word>
      <word>real_quick_ratio</word>
      <word>readlines</word>
      <word>readline</word>
      <word>read</word>
      <word>re</word>
      <word>raw_input</word>
      <word>ratio</word>
      <word>quick_ratio</word>
      <word>PY_SOURCE</word>
      <word>PY_RESOURCE</word>
      <word>PY_FROZEN</word>
      <word>PY_COMPILED</word>
      <word>push</word>
      <word>punctuation</word>
      <word>ps2</word>
      <word>ps1</word>
      <word>ProxyTypes</word>
      <word>ProxyType</word>
      <word>proxy</word>
      <word>printable</word>
      <word>print_tb</word>
      <word>print_stack</word>
      <word>print_last</word>
      <word>print_exception</word>
      <word>print_exc</word>
      <word>print</word>
      <word>PrettyPrinter</word>
      <word>prefix</word>
      <word>pprint</word>
      <word>pprint</word>
      <word>pos</word>
      <word>pos</word>
      <word>platform</word>
      <word>PKG_DIRECTORY</word>
      <word>PicklingError</word>
      <word>Pickler</word>
      <word>PickleError</word>
      <word>pickle</word>
      <word>pickle</word>
      <word>pformat</word>
      <word>pattern</word>
      <word>path</word>
      <word>pack</word>
      <word>OutputType</word>
      <word>os</word>
      <word>or_</word>
      <word>operator</word>
      <word>open</word>
      <word>octdigits</word>
      <word>numeric</word>
      <word>NotANumber</word>
      <word>not_</word>
      <word>NoneType</word>
      <word>noload</word>
      <word>new_module</word>
      <word>new</word>
      <word>neg</word>
      <word>ndiff</word>
      <word>name</word>
      <word>MutableString</word>
      <word>mul</word>
      <word>ModuleType</word>
      <word>modules</word>
      <word>module</word>
      <word>mod</word>
      <word>mirrored</word>
      <word>MethodType</word>
      <word>maxunicode</word>
      <word>maxtuple</word>
      <word>maxstring</word>
      <word>maxother</word>
      <word>maxlong</word>
      <word>maxlist</word>
      <word>maxlevel</word>
      <word>maxint</word>
      <word>maxdict</word>
      <word>match</word>
      <word>marshal</word>
      <word>maketrans</word>
      <word>lstrip</word>
      <word>lshift</word>
      <word>lowercase</word>
      <word>lower</word>
      <word>lookup</word>
      <word>lookup</word>
      <word>LongType</word>
      <word>lock_held</word>
      <word>loads</word>
      <word>loads</word>
      <word>load_source</word>
      <word>load_module</word>
      <word>load_dynamic</word>
      <word>load_compiled</word>
      <word>load</word>
      <word>load</word>
      <word>load</word>
      <word>ljust</word>
      <word>ListType</word>
      <word>linecache</word>
      <word>letters</word>
      <word>lastindex</word>
      <word>lastgroup</word>
      <word>last_value</word>
      <word>last_type</word>
      <word>last_traceback</word>
      <word>LambdaType</word>
      <word>joinfields</word>
      <word>join</word>
      <word>isSequenceType</word>
      <word>isrecursive</word>
      <word>isreadable</word>
      <word>isNumberType</word>
      <word>isMappingType</word>
      <word>isenabled</word>
      <word>isCallable</word>
      <word>IS_LINE_JUNK</word>
      <word>is_frozen</word>
      <word>IS_CHARACTER_JUNK</word>
      <word>is_builtin</word>
      <word>invert</word>
      <word>inv</word>
      <word>IntType</word>
      <word>InteractiveInterpreter</word>
      <word>InteractiveConsole</word>
      <word>interact</word>
      <word>interact</word>
      <word>InstanceType</word>
      <word>instancemethod</word>
      <word>instance</word>
      <word>inspect</word>
      <word>InputType</word>
      <word>init_frozen</word>
      <word>init_builtin</word>
      <word>IndexOf</word>
      <word>index</word>
      <word>import</word>
      <word>imp</word>
      <word>if</word>
      <word>htmlxmlsql2000db</word>
      <word>hexversion</word>
      <word>hexdigits</word>
      <word>groups</word>
      <word>groupindex</word>
      <word>groupdict</word>
      <word>group</word>
      <word>getwriter</word>
      <word>getweakrefs</word>
      <word>getweakrefcount</word>
      <word>getvalue</word>
      <word>getsourcelines</word>
      <word>getsourcefile</word>
      <word>getsource</word>
      <word>getslice</word>
      <word>getrefcount</word>
      <word>getrecursionlimit</word>
      <word>getreader</word>
      <word>getouterframes</word>
      <word>getmro</word>
      <word>getmodule</word>
      <word>getline</word>
      <word>getitem</word>
      <word>getinnerframes</word>
      <word>getframeinfo</word>
      <word>getfile</word>
      <word>getencoder</word>
      <word>getdoc</word>
      <word>getdlopenflags</word>
      <word>getdefaultencoding</word>
      <word>getdecoder</word>
      <word>getcomments</word>
      <word>getclasstree</word>
      <word>getargvalues</word>
      <word>getargspec</word>
      <word>get_threshold</word>
      <word>get_suffixes</word>
      <word>get_referrers</word>
      <word>get_opcodes</word>
      <word>get_objects</word>
      <word>get_matching_blocks</word>
      <word>get_magic</word>
      <word>get_debug</word>
      <word>get_close_matches</word>
      <word>GeneratorType</word>
      <word>gc</word>
      <word>garbage</word>
      <word>FunctionType</word>
      <word>function</word>
      <word>FrameType</word>
      <word>fpectl</word>
      <word>formatwarning</word>
      <word>formatargvalues</word>
      <word>formatargspec</word>
      <word>format_tb</word>
      <word>format_stack</word>
      <word>format_list</word>
      <word>format_exception_only</word>
      <word>format_exception</word>
      <word>floordiv</word>
      <word>FloatType</word>
      <word>FloatingPointError</word>
      <word>flags</word>
      <word>fix</word>
      <word>finditer</word>
      <word>findall</word>
      <word>find_module</word>
      <word>find_longest_match</word>
      <word>find</word>
      <word>filterwarnings</word>
      <word>FileType</word>
      <word>factory</word>
      <word>extract_tb</word>
      <word>extract_stack</word>
      <word>expandtabs</word>
      <word>expand</word>
      <word>exitfunc</word>
      <word>exit</word>
      <word>executable</word>
      <word>exec_prefix</word>
      <word>exception</word>
      <word>excepthook</word>
      <word>exc_value</word>
      <word>exc_type</word>
      <word>exc_traceback</word>
      <word>exc_info</word>
      <word>endpos</word>
      <word>end</word>
      <word>EncodedFile</word>
      <word>encode</word>
      <word>enable</word>
      <word>else</word>
      <word>EllipsisType</word>
      <word>dumps</word>
      <word>dumps</word>
      <word>dump</word>
      <word>dump</word>
      <word>dump</word>
      <word>dllhandle</word>
      <word>div</word>
      <word>displayhook</word>
      <word>disable</word>
      <word>digits</word>
      <word>digit</word>
      <word>Differ</word>
      <word>DictType</word>
      <word>DictionaryType</word>
      <word>delslice</word>
      <word>delitem</word>
      <word>def</word>
      <word>decomposition</word>
      <word>decode</word>
      <word>decimal</word>
      <word>DEBUG_UNCOLLECTABLE</word>
      <word>DEBUG_STATS</word>
      <word>DEBUG_SAVEALL</word>
      <word>DEBUG_OBJECTS</word>
      <word>DEBUG_LEAK</word>
      <word>DEBUG_INSTANCES</word>
      <word>DEBUG_COLLECTABLE</word>
      <word>data</word>
      <word>currentframe</word>
      <word>cPickle</word>
      <word>countOf</word>
      <word>count</word>
      <word>copyright</word>
      <word>copy_reg</word>
      <word>copy</word>
      <word>Cookie</word>
      <word>contains</word>
      <word>constructor</word>
      <word>concat</word>
      <word>ComplexType</word>
      <word>compile_command</word>
      <word>compile_command</word>
      <word>Compile</word>
      <word>compare</word>
      <word>CommandCompiler</word>
      <word>combining</word>
      <word>collect</word>
      <word>CodeType</word>
      <word>codeop</word>
      <word>code</word>
      <word>code</word>
      <word>close</word>
      <word>clecheckcache</word>
      <word>clear_memo</word>
      <word>ClassType</word>
      <word>classobj</word>
      <word>class</word>
      <word>cgi</word>
      <word>center</word>
      <word>category</word>
      <word>capwords</word>
      <word>capitalize</word>
      <word>CallableProxyType</word>
      <word>calcsize</word>
      <word>C_EXTENSION</word>
      <word>C_BUILTIN</word>
      <word>byteorder</word>
      <word>BuiltinMethodType</word>
      <word>BuiltinFunctionType</word>
      <word>builtin_module_names</word>
      <word>BufferType</word>
      <word>bidirectional</word>
      <word>atol</word>
      <word>atoi</word>
      <word>atof</word>
      <word>atexit</word>
      <word>ascii_uppercase</word>
      <word>ascii_lowercase</word>
      <word>ascii_letters</word>
      <word>aRepr</word>
      <word>and_</word>
      <word>add</word>
      <word>abs</word>
      <word>_getframe</word>
      <word>__xor__</word>
      <word>__truediv__</word>
      <word>__sub__</word>
      <word>__stdout__</word>
      <word>__stdin__</word>
      <word>__stderr__</word>
      <word>__setslice__</word>
      <word>__setitem__</word>
      <word>__rshift__</word>
      <word>__repeat__</word>
      <word>__pos__</word>
      <word>__or__</word>
      <word>__neg__</word>
      <word>__ne__</word>
      <word>__mul__</word>
      <word>__mod__</word>
      <word>__main__</word>
      <word>__lt__</word>
      <word>__lshift__</word>
      <word>__le__</word>
      <word>__invert__</word>
      <word>__inv__</word>
      <word>__gt__</word>
      <word>__getslice__</word>
      <word>__getitem__</word>
      <word>__ge__</word>
      <word>__floordiv__</word>
      <word>__excepthook__</word>
      <word>__eq__</word>
      <word>__div__</word>
      <word>__displayhook__</word>
      <word>__delslice__</word>
      <word>__delitem__</word>
      <word>__contains__</word>
      <word>__concat__</word>
      <word>__builtin__</word>
      <word>__and__</word>
      <word>__add__</word>
      <word>__abs__</word>
    </pattern>
  </definition>
  <definition name=""Ruby"" caseSensitive=""true"">
    <default>
      <font name=""Menlo"" size=""11"" style=""normal"" foreColor=""black"" backColor=""transparent""/>
    </default>
    <pattern name=""Comment"" type=""block"" beginsWith=""#"" endsWith=""\n"">
      <font name=""Menlo"" size=""11"" style=""normal"" foreColor=""green"" backColor=""transparent""/>
    </pattern>
    <pattern name=""MultiLineComment"" type=""block"" beginsWith=""/*"" endsWith=""*/"">
      <font name=""Menlo"" size=""11"" style=""normal"" foreColor=""green"" backColor=""transparent""/>
    </pattern>
    <pattern name=""String"" type=""block"" beginsWith=""&quot;"" endsWith=""&quot;"" escapesWith=""\"">
      <font name=""Menlo"" size=""11"" style=""normal"" foreColor=""#666666"" backColor=""transparent""/>
    </pattern>
    <pattern name=""WordGroup01"" type=""word"">
      <font name=""Menlo"" size=""11"" style=""normal"" foreColor=""blue"" backColor=""transparent""/>
      <word>yield</word>
      <word>while</word>
      <word>when</word>
      <word>until</word>
      <word>unless</word>
      <word>undef</word>
      <word>true</word>
      <word>then</word>
      <word>super</word>
      <word>self</word>
      <word>return</word>
      <word>retry</word>
      <word>rescue</word>
      <word>redo</word>
      <word>or</word>
      <word>not</word>
      <word>nil</word>
      <word>next</word>
      <word>module</word>
      <word>in</word>
      <word>if</word>
      <word>for</word>
      <word>false</word>
      <word>ensure</word>
      <word>END</word>
      <word>end</word>
      <word>elsif</word>
      <word>else</word>
      <word>do</word>
      <word>defined?</word>
      <word>def</word>
      <word>class</word>
      <word>case</word>
      <word>break</word>
      <word>BEGIN</word>
      <word>begin</word>
      <word>and</word>
      <word>alias</word>
      <word>__LINE__</word>
      <word>__FILE__</word>
    </pattern>
    <pattern name=""WordGroup02"" type=""word"">
      <font name=""Menlo"" size=""11"" style=""normal"" foreColor=""darkblue"" backColor=""transparent""/>
      <word>untrace_var</word>
      <word>trap</word>
      <word>trace_var</word>
      <word>throw</word>
      <word>test</word>
      <word>system</word>
      <word>syscall</word>
      <word>sub</word>
      <word>sub</word>
      <word>String</word>
      <word>srand</word>
      <word>sprintf</word>
      <word>split</word>
      <word>sleep</word>
      <word>select</word>
      <word>require</word>
      <word>readlines</word>
      <word>readline</word>
      <word>rand</word>
      <word>raise</word>
      <word>puts</word>
      <word>putc</word>
      <word>proc</word>
      <word>printf</word>
      <word>print</word>
      <word>p</word>
      <word>open</word>
      <word>loop</word>
      <word>local_variables</word>
      <word>load</word>
      <word>lamda</word>
      <word>iterator</word>
      <word>Integer</word>
      <word>import</word>
      <word>hex</word>
      <word>gt</word>
      <word>gsub</word>
      <word>gsub</word>
      <word>goto</word>
      <word>gmtime</word>
      <word>global_variables</word>
      <word>glob</word>
      <word>gets</word>
      <word>format</word>
      <word>fork</word>
      <word>Float</word>
      <word>fail</word>
      <word>exp</word>
      <word>exit</word>
      <word>exit</word>
      <word>exists</word>
      <word>exec</word>
      <word>eval</word>
      <word>chop</word>
      <word>chop</word>
      <word>chomp</word>
      <word>chomp</word>
      <word>catch</word>
      <word>caller</word>
      <word>binding</word>
      <word>autoload</word>
      <word>at_exit</word>
      <word>Array</word>
    </pattern>
    <pattern name=""WordGroup03"" type=""word"">
      <font name=""Menlo"" size=""11"" style=""normal"" foreColor=""red"" backColor=""transparent""/>
      <word>ZeroDivisionError</word>
      <word>TypeError</word>
      <word>Time</word>
      <word>ThreadError</word>
      <word>SystemStackError</word>
      <word>SystemExit</word>
      <word>SystemCallError</word>
      <word>SyntaxError</word>
      <word>Struct</word>
      <word>String</word>
      <word>StandardError</word>
      <word>SignalException</word>
      <word>SecurityError</word>
      <word>RuntimeError</word>
      <word>Regexp</word>
      <word>Range</word>
      <word>Proc</word>
      <word>Object</word>
      <word>Numeric</word>
      <word>NotImplementError</word>
      <word>NilClass</word>
      <word>NameError</word>
      <word>Module</word>
      <word>MatchingData</word>
      <word>LocalJumpError</word>
      <word>LoadError</word>
      <word>IOError</word>
      <word>IO</word>
      <word>Interrupt</word>
      <word>Integer</word>
      <word>IndexError</word>
      <word>Hash</word>
      <word>Float</word>
      <word>Fixnum</word>
      <word>File</word>
      <word>fatal</word>
      <word>Exception</word>
      <word>EOFError</word>
      <word>Dir</word>
      <word>Data</word>
      <word>Class</word>
      <word>Bignum</word>
      <word>Array</word>
      <word>ArgumentError</word>
    </pattern>
  </definition>
  <definition name=""SQL"" caseSensitive=""false"">
    <default>
      <font name=""Menlo"" size=""11"" style=""normal"" foreColor=""black"" backColor=""transparent""/>
    </default>
    <pattern name=""SysTable"" type=""word"">
      <font name=""Menlo"" size=""11"" style=""normal"" foreColor=""lawngreen"" backColor=""transparent""/>
      <word>sysaltfiles</word>
      <word>syslockinfo</word>
      <word>syscacheobjects</word>
      <word>syslogins</word>
      <word>sysxlogins</word>
      <word>syscharsets</word>
      <word>sysmessages</word>
      <word>sysconfigures</word>
      <word>sysoledbusers</word>
      <word>syscurconfigs</word>
      <word>sysperfinfo</word>
      <word>sysdatabases</word>
      <word>sysprocesses</word>
      <word>sysdevices</word>
      <word>sysremotelogins</word>
      <word>syslanguages</word>
      <word>sysservers</word>
      <word>syscolumns</word>
      <word>sysindexkeys</word>
      <word>syscomments</word>
      <word>sysmembers</word>
      <word>sysconstraints</word>
      <word>sysremote_column_privileges</word>
      <word>sysobjects</word>
      <word>sysdepends</word>
      <word>syspermissions</word>
      <word>sysfilegroups</word>
      <word>sysprotects</word>
      <word>sysfiles</word>
      <word>sysreferences</word>
      <word>sysforeignkeys</word>
      <word>sysTypes</word>
      <word>sysfulltextcatalogs</word>
      <word>sysusers</word>
      <word>sysindexes</word>
      <word>sysalerts</word>
      <word>sysjobsteps</word>
      <word>syscategories</word>
      <word>sysnotifications</word>
      <word>sysdownloadlist</word>
      <word>sysoperators</word>
      <word>sysjobhistory</word>
      <word>systargetservergroupmembers</word>
      <word>sysjobs</word>
      <word>systargetservergroups</word>
      <word>sysjobschedules</word>
      <word>systargetservers</word>
      <word>sysjobservers</word>
      <word>systaskids</word>
    </pattern>
    <pattern name=""SysProc"" type=""word"">
      <font name=""Menlo"" size=""11"" style=""normal"" foreColor=""brown"" backColor=""transparent""/>
      <word>sp_bindsession</word>
      <word>sp_column_privileges_ex</word>
      <word>sp_createorphan</word>
      <word>sp_cursor</word>
      <word>sp_cursorclose</word>
      <word>sp_cursorexecute</word>
      <word>sp_cursorfetch</word>
      <word>sp_cursoropen</word>
      <word>sp_cursoroption</word>
      <word>sp_cursorprepare</word>
      <word>sp_cursorprepexec</word>
      <word>sp_cursorunprepare</word>
      <word>sp_droporphans</word>
      <word>sp_execute</word>
      <word>sp_executesql</word>
      <word>sp_fulltext_getdata</word>
      <word>sp_getbindtoken</word>
      <word>sp_GetMBCSCharLen</word>
      <word>sp_getschemalock</word>
      <word>sp_IsMBCSLeadByte</word>
      <word>sp_MSgetversion</word>
      <word>sp_OACreate</word>
      <word>sp_OADestroy</word>
      <word>sp_OAGetErrorInfo</word>
      <word>sp_OAGetProperty</word>
      <word>sp_OAMethod</word>
      <word>sp_OASetProperty</word>
      <word>sp_OAStop</word>
      <word>sp_prepare</word>
      <word>sp_prepexec</word>
      <word>sp_prepexecrpc</word>
      <word>sp_refreshview</word>
      <word>sp_releaseschemalock</word>
      <word>sp_replcmds</word>
      <word>sp_replcounters</word>
      <word>sp_repldone</word>
      <word>sp_replflush</word>
      <word>sp_replincrementlsn</word>
      <word>sp_replpostcmd</word>
      <word>sp_replpostschema</word>
      <word>sp_replpostsyncstatus</word>
      <word>sp_replsendtoqueue</word>
      <word>sp_replsetoriginator</word>
      <word>sp_replsetsyncstatus</word>
      <word>sp_repltrans</word>
      <word>sp_replupdateschema</word>
      <word>sp_replwritetovarbin</word>
      <word>sp_reset_connection</word>
      <word>sp_resyncexecute</word>
      <word>sp_resyncexecutesql</word>
      <word>sp_resyncprepare</word>
      <word>sp_resyncuniquetable</word>
      <word>sp_sdidebug</word>
      <word>sp_trace_create</word>
      <word>sp_trace_generateevent</word>
      <word>sp_trace_setevent</word>
      <word>sp_trace_setfilter</word>
      <word>sp_trace_setstatus</word>
      <word>sp_unprepare</word>
      <word>sp_xml_preparedocument</word>
      <word>sp_xml_removedocument</word>
      <word>xp_adsirequest</word>
      <word>xp_availablemedia</word>
      <word>xp_cleanupwebtask</word>
      <word>xp_cmdshell</word>
      <word>xp_controlqueueservice</word>
      <word>xp_convertwebtask</word>
      <word>xp_createprivatequeue</word>
      <word>xp_createqueue</word>
      <word>xp_decodequeuecmd</word>
      <word>xp_deletemail</word>
      <word>xp_deleteprivatequeue</word>
      <word>xp_deletequeue</word>
      <word>xp_dirtree</word>
      <word>xp_displayparamstmt</word>
      <word>xp_displayqueuemesgs</word>
      <word>xp_dropwebtask</word>
      <word>xp_dsninfo</word>
      <word>xp_enum_activescriptengines</word>
      <word>xp_enum_oledb_providers</word>
      <word>xp_enumcodepages</word>
      <word>xp_enumdsn</word>
      <word>xp_enumerrorlogs</word>
      <word>xp_enumgroups</word>
      <word>xp_eventlog</word>
      <word>xp_execresultset</word>
      <word>xp_fileexist</word>
      <word>xp_findnextmsg</word>
      <word>xp_fixeddrives</word>
      <word>xp_get_mapi_default_profile</word>
      <word>xp_get_mapi_profiles</word>
      <word>xp_get_tape_devices</word>
      <word>xp_GetAdminGroupName</word>
      <word>xp_getfiledetails</word>
      <word>xp_getnetname</word>
      <word>xp_getprotocoldllinfo</word>
      <word>xp_instance_regaddmultistring</word>
      <word>xp_instance_regdeletekey</word>
      <word>xp_instance_regdeletevalue</word>
      <word>xp_instance_regenumkeys</word>
      <word>xp_instance_regenumvalues</word>
      <word>xp_instance_regread</word>
      <word>xp_instance_regremovemultistring</word>
      <word>xp_instance_regwrite</word>
      <word>xp_intersectbitmaps</word>
      <word>xp_IsNTAdmin</word>
      <word>xp_logevent</word>
      <word>xp_loginconfig</word>
      <word>xp_makecab</word>
      <word>xp_makewebtask</word>
      <word>xp_mapdown_bitmap</word>
      <word>xp_mergelineages</word>
      <word>xp_mergexpusage</word>
      <word>xp_MSADEnabled</word>
      <word>xp_MSADSIObjReg</word>
      <word>xp_MSADSIObjRegDB</word>
      <word>xp_MSADSIReg</word>
      <word>xp_MSFullText</word>
      <word>xp_MSLocalSystem</word>
      <word>xp_MSnt2000</word>
      <word>xp_MSplatform</word>
      <word>xp_msver</word>
      <word>xp_msx_enlist</word>
      <word>xp_ntsec_enumdomains</word>
      <word>xp_oledbinfo</word>
      <word>xp_ORbitmap</word>
      <word>xp_peekqueue</word>
      <word>xp_printstatements</word>
      <word>xp_prop_oledb_provider</word>
      <word>xp_proxiedmetadata</word>
      <word>xp_qv</word>
      <word>xp_readerrorlog</word>
      <word>xp_readmail</word>
      <word>xp_readpkfromqueue</word>
      <word>xp_readpkfromvarbin</word>
      <word>xp_readwebtask</word>
      <word>xp_regaddmultistring</word>
      <word>xp_regdeletekey</word>
      <word>xp_regdeletevalue</word>
      <word>xp_regenumkeys</word>
      <word>xp_regenumvalues</word>
      <word>xp_regread</word>
      <word>xp_regremovemultistring</word>
      <word>xp_regwrite</word>
      <word>xp_repl_convert_encrypt</word>
      <word>xp_repl_encrypt</word>
      <word>xp_repl_help_connect</word>
      <word>xp_replproberemsrv</word>
      <word>xp_resetqueue</word>
      <word>xp_runwebtask</word>
      <word>xp_sendmail</word>
      <word>xp_servicecontrol</word>
      <word>xp_SetSQLSecurity</word>
      <word>xp_showcolv</word>
      <word>xp_showlineage</word>
      <word>xp_sprintf</word>
      <word>xp_sqlagent_enum_jobs</word>
      <word>xp_sqlagent_is_starting</word>
      <word>xp_sqlagent_monitor</word>
      <word>xp_sqlagent_notify</word>
      <word>xp_sqlagent_param</word>
      <word>xp_sqlagent_proxy_account</word>
      <word>xp_sqlmaint</word>
      <word>xp_sscanf</word>
      <word>xp_startmail</word>
      <word>xp_stopmail</word>
      <word>xp_subdirs</word>
      <word>xp_terminate_process</word>
      <word>xp_test_mapi_profile</word>
      <word>xp_unc_to_drive</word>
      <word>xp_unpackcab</word>
      <word>xp_updateFTSSQLAccount</word>
      <word>xp_userlock</word>
      <word>xp_varbintohexstr</word>
    </pattern>
    <pattern name=""GlobalVariable"" type=""word"">
      <font name=""Menlo"" size=""11"" style=""normal"" foreColor=""fuchsia"" backColor=""transparent""/>
      <word>@@DATEFIRST</word>
      <word>@@OPTIONS</word>
      <word>@@DBTS</word>
      <word>@@REMSERVER</word>
      <word>@@LANGID</word>
      <word>@@SERVERName</word>
      <word>@@LANGUAGE</word>
      <word>@@SERVICEName</word>
      <word>@@LOCK_TIMEOUT</word>
      <word>@@SPID</word>
      <word>@@MAX_CONNECTIONS</word>
      <word>@@TEXTSIZE</word>
      <word>@@MAX_PRECISION</word>
      <word>@@VERSION</word>
      <word>@@NESTLEVEL</word>
      <word>@@CURSOR_ROWS</word>
      <word>@@FETCH_STATUS</word>
      <word>@@PROCID</word>
      <word>@@IDENTITY</word>
      <word>@@ROWCOUNT</word>
      <word>@@ERROR</word>
      <word>@@TRANCOUNT</word>
      <word>@@CONNECTIONS</word>
      <word>@@PACK_RECEIVED</word>
      <word>@@CPU_BUSY</word>
      <word>@@PACK_SENT</word>
      <word>@@TIMETICKS</word>
      <word>@@IDLE</word>
      <word>@@TOTAL_ERRORS</word>
      <word>@@IO_BUSY</word>
      <word>@@TOTAL_READ</word>
      <word>@@PACKET_ERRORS</word>
      <word>@@TOTAL_WRITE</word>
    </pattern>
    <pattern name=""ReservedKeyword"" type=""word"">
      <font name=""Menlo"" size=""11"" style=""normal"" foreColor=""blue"" backColor=""transparent""/>
      <word>sysname</word>
      <word>ADD</word>
      <word>EXCEPT</word>
      <word>PERCENT</word>
      <word>EXEC</word>
      <word>PLAN</word>
      <word>ALTER</word>
      <word>EXECUTE</word>
      <word>PRECISION</word>
      <word>PRIMARY</word>
      <word>EXIT</word>
      <word>PRINT</word>
      <word>AS</word>
      <word>FETCH</word>
      <word>NOCOUNT</word>
      <word>PROC</word>
      <word>ASC</word>
      <word>FILE</word>
      <word>PROCEDURE</word>
      <word>AUTHORIZATION</word>
      <word>FILLFACTOR</word>
      <word>PUBLIC</word>
      <word>BACKUP</word>
      <word>FOR</word>
      <word>RAISE</word>
      <word>RAISERROR</word>
      <word>BEGIN</word>
      <word>FOREIGN</word>
      <word>READ</word>
      <word>FREETEXT</word>
      <word>READTEXT</word>
      <word>BREAK</word>
      <word>FREETEXTTABLE</word>
      <word>RECONFIGURE</word>
      <word>BROWSE</word>
      <word>FROM</word>
      <word>REFERENCES</word>
      <word>BULK</word>
      <word>FULL</word>
      <word>REPLICATION</word>
      <word>BY</word>
      <word>FUNCTION</word>
      <word>RESTORE</word>
      <word>CASCADE</word>
      <word>GOTO</word>
      <word>RESTRICT</word>
      <word>GRANT</word>
      <word>RETURN</word>
      <word>CHECK</word>
      <word>GROUP</word>
      <word>REVOKE</word>
      <word>CHECKPOINT</word>
      <word>HAVING</word>
      <word>CLOSE</word>
      <word>HOLDLOCK</word>
      <word>ROLLBACK</word>
      <word>CLUSTERED</word>
      <word>IDENTITY</word>
      <word>IDENTITY_INSERT</word>
      <word>ROWGUIDCOL</word>
      <word>COLLATE</word>
      <word>IDENTITYCOL</word>
      <word>RULE</word>
      <word>COLUMN</word>
      <word>IF</word>
      <word>SAVE</word>
      <word>COMMIT</word>
      <word>SCHEMA</word>
      <word>COMPUTE</word>
      <word>INDEX</word>
      <word>SELECT</word>
      <word>CONSTRAINT</word>
      <word>INNER</word>
      <word>CONTAINS</word>
      <word>INSERT</word>
      <word>SET</word>
      <word>CONTAINSTABLE</word>
      <word>INTERSECT</word>
      <word>SETUSER</word>
      <word>CONTINUE</word>
      <word>INTO</word>
      <word>SHUTDOWN</word>
      <word>IS</word>
      <word>CREATE</word>
      <word>STATISTICS</word>
      <word>KEY</word>
      <word>CURRENT</word>
      <word>KILL</word>
      <word>TABLE</word>
      <word>CURRENT_DATE</word>
      <word>TEXTSIZE</word>
      <word>CURRENT_TIME</word>
      <word>THEN</word>
      <word>LINENO</word>
      <word>TO</word>
      <word>LOAD</word>
      <word>TOP</word>
      <word>CURSOR</word>
      <word>NATIONAL</word>
      <word>TRAN</word>
      <word>DATABASE</word>
      <word>NOCHECK</word>
      <word>TRANSACTION</word>
      <word>DBCC</word>
      <word>NONCLUSTERED</word>
      <word>TRIGGER</word>
      <word>DEALLOCATE</word>
      <word>TRUNCATE</word>
      <word>DECLARE</word>
      <word>TSEQUAL</word>
      <word>DEFAULT</word>
      <word>UNION</word>
      <word>DELETE</word>
      <word>OF</word>
      <word>UNIQUE</word>
      <word>DENY</word>
      <word>OFF</word>
      <word>UPDATE</word>
      <word>DESC</word>
      <word>OFFSETS</word>
      <word>UPDATETEXT</word>
      <word>DISK</word>
      <word>ON</word>
      <word>USE</word>
      <word>DISTINCT</word>
      <word>OPEN</word>
      <word>DISTRIBUTED</word>
      <word>OPENDATASOURCE</word>
      <word>VALUES</word>
      <word>DOUBLE</word>
      <word>OPENQUERY</word>
      <word>VARYING</word>
      <word>DROP</word>
      <word>OPENROWSET</word>
      <word>VIEW</word>
      <word>DUMMY</word>
      <word>OPENXML</word>
      <word>WAITFOR</word>
      <word>DUMP</word>
      <word>OPTION</word>
      <word>WHEN</word>
      <word>ELSE</word>
      <word>WHERE</word>
      <word>END</word>
      <word>ORDER</word>
      <word>WHILE</word>
      <word>ERRLVL</word>
      <word>WITH</word>
      <word>ESCAPE</word>
      <word>OVER</word>
      <word>WRITETEXT</word>
      <word>QUOTED_IDENTIFIER</word>
      <word>ANSI_NULLS</word>
      <word>OUTPUT</word>
      <word>OUT</word>
    </pattern>
    <pattern name=""Function"" type=""word"">
      <font name=""Menlo"" size=""11"" style=""normal"" foreColor=""fuchsia"" backColor=""transparent""/>
      <word>AVG</word>
      <word>MAX</word>
      <word>BINARY_CHECKSUM</word>
      <word>MIN</word>
      <word>CHECKSUM</word>
      <word>SUM</word>
      <word>CHECKSUM_AGG</word>
      <word>STDEV</word>
      <word>COUNT</word>
      <word>STDEVP</word>
      <word>COUNT_BIG</word>
      <word>VAR</word>
      <word>GROUPING</word>
      <word>VARP</word>
      <word>CURSOR_STATUS</word>
      <word>DATEADD</word>
      <word>DATEDIFF</word>
      <word>DATEName</word>
      <word>DATEPART</word>
      <word>DAY</word>
      <word>GETDATE</word>
      <word>GETUTCDATE</word>
      <word>MONTH</word>
      <word>YEAR</word>
      <word>ABS</word>
      <word>DEGREES</word>
      <word>RAND</word>
      <word>ACOS</word>
      <word>EXP</word>
      <word>ROUND</word>
      <word>ASIN</word>
      <word>FLOOR</word>
      <word>SIGN</word>
      <word>ATAN</word>
      <word>LOG</word>
      <word>SIN</word>
      <word>ATN2</word>
      <word>LOG10</word>
      <word>SQUARE</word>
      <word>CEILING</word>
      <word>PI</word>
      <word>SQRT</word>
      <word>COS</word>
      <word>POWER</word>
      <word>TAN</word>
      <word>COT</word>
      <word>RADIANS</word>
      <word>COL_LENGTH</word>
      <word>COL_Name</word>
      <word>FULLTEXTCATALOGPROPERTY</word>
      <word>COLUMNPROPERTY</word>
      <word>FULLTEXTSERVICEPROPERTY</word>
      <word>DATABASEPROPERTY</word>
      <word>INDEX_COL</word>
      <word>DATABASEPROPERTYEX</word>
      <word>INDEXKEY_PROPERTY</word>
      <word>DB_ID</word>
      <word>INDEXPROPERTY</word>
      <word>DB_Name</word>
      <word>OBJECT_ID</word>
      <word>FILE_ID</word>
      <word>OBJECT_Name</word>
      <word>FILE_Name</word>
      <word>OBJECTPROPERTY</word>
      <word>FILEGROUP_ID</word>
      <word>FILEGROUP_Name</word>
      <word>SQL_VARIANT_PROPERTY</word>
      <word>FILEGROUPPROPERTY</word>
      <word>TypePROPERTY</word>
      <word>FILEPROPERTY</word>
      <word>IS_SRVROLEMEMBER</word>
      <word>SUSER_SID</word>
      <word>SUSER_SName</word>
      <word>USER_ID</word>
      <word>HAS_DBACCESS</word>
      <word>USER</word>
      <word>IS_MEMBER</word>
      <word>ASCII</word>
      <word>NCHAR</word>
      <word>SOUNDEX</word>
      <word>CHAR</word>
      <word>PATINDEX</word>
      <word>SPACE</word>
      <word>CHARINDEX</word>
      <word>REPLACE</word>
      <word>STR</word>
      <word>DIFFERENCE</word>
      <word>QUOTEName</word>
      <word>STUFF</word>
      <word>LEFT</word>
      <word>REPLICATE</word>
      <word>SUBSTRING</word>
      <word>LEN</word>
      <word>REVERSE</word>
      <word>UNICODE</word>
      <word>LOWER</word>
      <word>RIGHT</word>
      <word>UPPER</word>
      <word>LTRIM</word>
      <word>RTRIM</word>
      <word>APP_Name</word>
      <word>CASE</word>
      <word>CAST</word>
      <word>CONVERT</word>
      <word>COALESCE</word>
      <word>COLLATIONPROPERTY</word>
      <word>CURRENT_TIMESTAMP</word>
      <word>CURRENT_USER</word>
      <word>DATALENGTH</word>
      <word>FORMATMESSAGE</word>
      <word>GETANSINULL</word>
      <word>HOST_ID</word>
      <word>HOST_Name</word>
      <word>IDENT_CURRENT</word>
      <word>IDENT_INCRIDENT_SEED</word>
      <word>IDENTITY</word>
      <word>ISDATE</word>
      <word>ISNULL</word>
      <word>ISNUMERIC</word>
      <word>NEWID</word>
      <word>NULLIF</word>
      <word>PARSEName</word>
      <word>PERMISSIONS</word>
      <word>ROWCOUNT_BIG</word>
      <word>SCOPE_IDENTITY</word>
      <word>SERVERPROPERTY</word>
      <word>SESSIONPROPERTY</word>
      <word>SESSION_USER</word>
      <word>STATS_DATE</word>
      <word>SYSTEM_USER</word>
      <word>USER_Name</word>
      <word>PATINDEX</word>
      <word>TEXTPTR</word>
      <word>TEXTVALID</word>
      <word>CASE</word>
      <word>RIGHT</word>
      <word>COALESCE</word>
      <word>SESSION_USER</word>
      <word>CONVERT</word>
      <word>SYSTEM_USER</word>
      <word>LEFT</word>
      <word>CURRENT_TIMESTAMP</word>
      <word>CURRENT_USER</word>
      <word>NULLIF</word>
      <word>USER</word>
    </pattern>
    <pattern name=""DataType"" type=""word"">
      <font name=""Menlo"" size=""11"" style=""normal"" foreColor=""blue"" backColor=""transparent""/>
      <word>bigint</word>
      <word>int</word>
      <word>smallint</word>
      <word>tinyint</word>
      <word>bit</word>
      <word>decimal</word>
      <word>numeric</word>
      <word>money</word>
      <word>smallmoney</word>
      <word>float</word>
      <word>real</word>
      <word>datetime</word>
      <word>smalldatetime</word>
      <word>char</word>
      <word>varchar</word>
      <word>text</word>
      <word>nchar</word>
      <word>nvarchar</word>
      <word>ntext</word>
      <word>binary</word>
      <word>varbinary</word>
      <word>image</word>
      <word>cursor</word>
      <word>sql_variant</word>
      <word>table</word>
      <word>timestamp</word>
      <word>uniqueidentifier</word>
    </pattern>
    <pattern name=""Operator"" type=""word"">
      <font name=""Menlo"" size=""11"" style=""normal"" foreColor=""silver"" backColor=""transparent""/>
      <word>ALL</word>
      <word>AND</word>
      <word>EXISTS</word>
      <word>ANY</word>
      <word>BETWEEN</word>
      <word>IN</word>
      <word>SOME</word>
      <word>JOIN</word>
      <word>CROSS</word>
      <word>LIKE</word>
      <word>NOT</word>
      <word>NULL</word>
      <word>OR</word>
      <word>OUTER</word>
    </pattern>
    <pattern name=""SystemFunction"" type=""word"">
      <font name=""Menlo"" size=""11"" style=""normal"" foreColor=""brown"" backColor=""transparent""/>
      <word>fn_listextendedproperty</word>
      <word>fn_trace_getinfo</word>
      <word>fn_trace_gettable</word>
      <word>fn_trace_geteventinfo</word>
      <word>fn_trace_getfilterinfo</word>
      <word>fn_helpcollations</word>
      <word>fn_servershareddrives</word>
      <word>fn_virtualfilestats</word>
      <word>fn_virtualfilestats</word>
    </pattern>
    <pattern name=""Comment"" type=""block"" beginsWith=""--"" endsWith=""\n"">
      <font name=""Menlo"" size=""11"" style=""normal"" foreColor=""teal"" backColor=""transparent""/>
    </pattern>
    <pattern name=""BlockComment"" type=""block"" beginsWith=""/*"" endsWith=""*/"">
      <font name=""Menlo"" size=""11"" style=""normal"" foreColor=""teal"" backColor=""transparent""/>
    </pattern>
    <pattern name=""String"" type=""block"" beginsWith=""'"" endsWith=""'"">
      <font name=""Menlo"" size=""11"" style=""normal"" foreColor=""red"" backColor=""transparent""/>
    </pattern>
  </definition>
  <definition name=""Visual Basic"" caseSensitive=""false"">
    <default>
      <font name=""Menlo"" size=""11"" style=""normal"" foreColor=""black"" backColor=""transparent""/>
    </default>
    <pattern name=""String"" type=""block"" beginsWith=""&quot;"" endsWith=""&quot;"">
      <font name=""Menlo"" size=""11"" style=""normal"" foreColor=""#666666"" backColor=""transparent""/>
    </pattern>
    <pattern name=""Comment"" type=""block"" beginsWith=""&apos;"" endsWith=""\n"">
      <font name=""Menlo"" size=""11"" style=""normal"" foreColor=""green"" backColor=""transparent""/>
    </pattern>
    <pattern name=""Keyword"" type=""word"">
      <font name=""Menlo"" size=""11"" style=""normal"" foreColor=""blue"" backColor=""transparent""/>
      <word>Xor</word>
      <word>WriteOnly</word>
      <word>WithEvents</word>
      <word>With</word>
      <word>Widening</word>
      <word>While</word>
      <word>When</word>
      <word>Wend</word>
      <word>Variant</word>
      <word>Using</word>
      <word>UShort</word>
      <word>ULong</word>
      <word>UInteger</word>
      <word>TypeOf</word>
      <word>TryCast</word>
      <word>Try</word>
      <word>True</word>
      <word>To</word>
      <word>Throw</word>
      <word>Then</word>
      <word>SyncLock</word>
      <word>Sub</word>
      <word>Structure</word>
      <word>String</word>
      <word>Stop</word>
      <word>Step</word>
      <word>Static</word>
      <word>Single</word>
      <word>Short</word>
      <word>Shared</word>
      <word>Shadows</word>
      <word>Set</word>
      <word>Select</word>
      <word>SByte</word>
      <word>Return</word>
      <word>Resume</word>
      <word>RemoveHandler</word>
      <word>REM</word>
      <word>ReDim</word>
      <word>ReadOnly</word>
      <word>RaiseEvent</word>
      <word>Public</word>
      <word>Protected</word>
      <word>Property</word>
      <word>Private</word>
      <word>Partial</word>
      <word>ParamArray</word>
      <word>Overrides</word>
      <word>Overridable</word>
      <word>Overloads</word>
      <word>OrElse</word>
      <word>Or</word>
      <word>Optional</word>
      <word>Option</word>
      <word>Operator</word>
      <word>On</word>
      <word>Of</word>
      <word>Object</word>
      <word>NotOverridable</word>
      <word>NotInheritable</word>
      <word>Nothing</word>
      <word>Not</word>
      <word>Next</word>
      <word>New</word>
      <word>Narrowing</word>
      <word>Namespace</word>
      <word>MyClass</word>
      <word>MyBase</word>
      <word>MustOverride</word>
      <word>MustInherit</word>
      <word>Module</word>
      <word>Mod</word>
      <word>Me</word>
      <word>Loop</word>
      <word>Long</word>
      <word>Like</word>
      <word>Lib</word>
      <word>Let</word>
      <word>IsNot</word>
      <word>Is</word>
      <word>Interface</word>
      <word>Integer</word>
      <word>Inherits</word>
      <word>In</word>
      <word>Imports</word>
      <word>Implements</word>
      <word>If</word>
      <word>Handles</word>
      <word>GoTo</word>
      <word>GoSub</word>
      <word>Global</word>
      <word>GetType</word>
      <word>Get</word>
      <word>Function</word>
      <word>Friend</word>
      <word>For</word>
      <word>Finally</word>
      <word>False</word>
      <word>Exit</word>
      <word>Event</word>
      <word>Error</word>
      <word>Erase</word>
      <word>Enum</word>
      <word>EndIf</word>
      <word>End</word>
      <word>ElseIf</word>
      <word>Else</word>
      <word>Each</word>
      <word>Double</word>
      <word>Do</word>
      <word>DirectCast</word>
      <word>Dim</word>
      <word>Delegate</word>
      <word>Default</word>
      <word>Declare</word>
      <word>Decimal</word>
      <word>Date</word>
      <word>CUShort</word>
      <word>CULng</word>
      <word>CUInt</word>
      <word>CType</word>
      <word>CStr</word>
      <word>CSng</word>
      <word>CShort</word>
      <word>CSByte</word>
      <word>Continue</word>
      <word>Const</word>
      <word>CObj</word>
      <word>CLng</word>
      <word>Class</word>
      <word>CInt</word>
      <word>Char</word>
      <word>CDec</word>
      <word>CDbl</word>
      <word>CDate</word>
      <word>CChar</word>
      <word>CByte</word>
      <word>CBool</word>
      <word>Catch</word>
      <word>Case</word>
      <word>Call</word>
      <word>ByVal</word>
      <word>Byte</word>
      <word>ByRef</word>
      <word>Boolean</word>
      <word>As</word>
      <word>AndAlso</word>
      <word>And</word>
      <word>Alias</word>
      <word>AddressOf</word>
      <word>AddHandler</word>
    </pattern>
  </definition>
  <definition name=""VBScript"" caseSensitive=""false"">
    <default>
      <font name=""Menlo"" size=""11"" style=""normal"" foreColor=""black"" backColor=""transparent""/>
    </default>
    <pattern name=""String"" type=""block"" beginsWith=""&quot;"" endsWith=""&quot;"">
      <font name=""Menlo"" size=""11"" style=""normal"" foreColor=""#666666"" backColor=""transparent""/>
    </pattern>
    <pattern name=""Comment"" type=""block"" beginsWith=""&apos;"" endsWith=""\n"">
      <font name=""Menlo"" size=""11"" style=""normal"" foreColor=""green"" backColor=""transparent""/>
    </pattern>
    <pattern name=""Keyword"" type=""word"">
      <font name=""Menlo"" size=""11"" style=""normal"" foreColor=""blue"" backColor=""transparent""/>
      <word>year</word>
      <word>xor</word>
      <word>write</word>
      <word>with</word>
      <word>with</word>
      <word>while</word>
      <word>wend</word>
      <word>weekdayname</word>
      <word>weekday</word>
      <word>vartype</word>
      <word>URLEncode</word>
      <word>ucase</word>
      <word>ubound</word>
      <word>trim</word>
      <word>Transfer</word>
      <word>timevalue</word>
      <word>timeserial</word>
      <word>timer</word>
      <word>time</word>
      <word>then</word>
      <word>textstream</word>
      <word>tan</word>
      <word>sub</word>
      <word>strreverse</word>
      <word>string</word>
      <word>strcomp</word>
      <word>sqr</word>
      <word>split</word>
      <word>space</word>
      <word>sin</word>
      <word>sgn</word>
      <word>setlocale</word>
      <word>SetComplete</word>
      <word>SetAbort</word>
      <word>set</word>
      <word>set</word>
      <word>Session</word>
      <word>session</word>
      <word>servervariables</word>
      <word>server</word>
      <word>select</word>
      <word>second</word>
      <word>scripting</word>
      <word>scriptengineminorversion</word>
      <word>scriptenginemajorversion</word>
      <word>scriptenginebuildversion</word>
      <word>scriptengine</word>
      <word>rtrim</word>
      <word>round</word>
      <word>rnd</word>
      <word>right</word>
      <word>rgb</word>
      <word>response</word>
      <word>request</word>
      <word>replace</word>
      <word>rem</word>
      <word>regexp</word>
      <word>Redirect</word>
      <word>redim</word>
      <word>randomize</word>
      <word>querystring</word>
      <word>public</word>
      <word>property</word>
      <word>private</word>
      <word>or</word>
      <word>option</word>
      <word>on</word>
      <word>oct</word>
      <word>objectcontext</word>
      <word>now</word>
      <word>nothing</word>
      <word>not</word>
      <word>next</word>
      <word>next</word>
      <word>msgbox</word>
      <word>monthname</word>
      <word>month</word>
      <word>mod</word>
      <word>minute</word>
      <word>mid</word>
      <word>matches</word>
      <word>match</word>
      <word>MapPath</word>
      <word>ltrim</word>
      <word>loop</word>
      <word>log</word>
      <word>loadpicture</word>
      <word>let</word>
      <word>len</word>
      <word>left</word>
      <word>lcase</word>
      <word>lbound</word>
      <word>join</word>
      <word>isobject</word>
      <word>isnumeric</word>
      <word>isnull</word>
      <word>isempty</word>
      <word>isdate</word>
      <word>isarray</word>
      <word>is</word>
      <word>int</word>
      <word>instrrev</word>
      <word>instr</word>
      <word>inputbox</word>
      <word>in</word>
      <word>imp</word>
      <word>if</word>
      <word>HTMLEncode</word>
      <word>hour</word>
      <word>hex</word>
      <word>getref</word>
      <word>getobject</word>
      <word>getlocale</word>
      <word>GetLastError</word>
      <word>get</word>
      <word>function</word>
      <word>formatpercent</word>
      <word>formatnumber</word>
      <word>formatdatetime</word>
      <word>formatcurrency</word>
      <word>form</word>
      <word>foreach</word>
      <word>for</word>
      <word>folders</word>
      <word>folder</word>
      <word>Flush</word>
      <word>fix</word>
      <word>filter</word>
      <word>filesystemobject</word>
      <word>files</word>
      <word>file</word>
      <word>explicit</word>
      <word>exp</word>
      <word>exit</word>
      <word>Execute</word>
      <word>execute</word>
      <word>eval</word>
      <word>error</word>
      <word>err</word>
      <word>erase</word>
      <word>eqv</word>
      <word>end</word>
      <word>else</word>
      <word>each</word>
      <word>drives</word>
      <word>drive</word>
      <word>do</word>
      <word>dim</word>
      <word>dictionary</word>
      <word>day</word>
      <word>datevalue</word>
      <word>cstr</word>
      <word>CreateObject</word>
      <word>createobject</word>
      <word>Contents.RemoveAll</word>
      <word>Contents.Remove</word>
      <word>const</word>
      <word>Clear</word>
      <word>class</word>
      <word>cint</word>
      <word>chr</word>
      <word>cdbl</word>
      <word>cdate</word>
      <word>ccur</word>
      <word>cbyte</word>
      <word>cbool</word>
      <word>case</word>
      <word>call</word>
      <word>BinaryWrite</word>
      <word>BinaryRead</word>
      <word>atn</word>
      <word>asperror</word>
      <word>asc</word>
      <word>array</word>
      <word>Application</word>
      <word>application</word>
      <word>AppendToLog</word>
      <word>and</word>
      <word>AddHeader</word>
      <word>add</word>
      <word>abs</word>
      <word>Abandon</word>
      <word>&lt;%</word>
      <word>%&gt;</word>
    </pattern>
  </definition>
  <definition name=""VB.NET"" caseSensitive=""false"">
    <default>
      <font name=""Menlo"" size=""11"" style=""normal"" foreColor=""black"" backColor=""transparent""/>
    </default>
    <pattern name=""Keyword"" type=""word"">
      <font name=""Menlo"" size=""11"" style=""normal"" foreColor=""blue"" backColor=""transparent""/>
      <word>Alias</word>
      <word>Ansi</word>
      <word>As</word>
      <word>Assembly</word>
      <word>Auto</word>
      <word>ByRef</word>
      <word>ByVal</word>
      <word>Case</word>
      <word>Default</word>
      <word>DirectCast</word>
      <word>Each</word>
      <word>Else</word>
      <word>ElseIf</word>
      <word>End</word>
      <word>Error</word>
      <word>Explicit</word>
      <word>False</word>
      <word>For</word>
      <word>Friend</word>
      <word>Handles</word>
      <word>Implements</word>
      <word>In</word>
      <word>Is</word>
      <word>Lib</word>
      <word>Loop</word>
      <word>Me</word>
      <word>Module</word>
      <word>MustInherit</word>
      <word>MustOverride</word>
      <word>MyBase</word>
      <word>MyClass</word>
      <word>New</word>
      <word>Next</word>
      <word>Nothing</word>
      <word>NotInheritable</word>
      <word>NotOverridable</word>
      <word>Off</word>
      <word>On</word>
      <word>Option</word>
      <word>Optional</word>
      <word>Overloads</word>
      <word>Overridable</word>
      <word>Overrides</word>
      <word>ParamArray</word>
      <word>Preserve</word>
      <word>Private</word>
      <word>Protected</word>
      <word>Public</word>
      <word>ReadOnly</word>
      <word>Resume</word>
      <word>Shadows</word>
      <word>Shared</word>
      <word>Static</word>
      <word>Step</word>
      <word>Strict</word>
      <word>Then</word>
      <word>To</word>
      <word>True</word>
      <word>TypeOf</word>
      <word>Unicode</word>
      <word>Until</word>
      <word>When</word>
      <word>While</word>
      <word>WithEvents</word>
      <word>WriteOnly</word>
    </pattern>
    <pattern name=""Statement"" type=""word"">
      <font name=""Menlo"" size=""11"" style=""normal"" foreColor=""blue"" backColor=""transparent""/>
      <word>AddHandler</word>
      <word>Call</word>
      <word>Class</word>
      <word>Const</word>
      <word>Declare</word>
      <word>Delegate</word>
      <word>Dim</word>
      <word>Do</word>
      <word>Loop</word>
      <word>End</word>
      <word>Enum</word>
      <word>Erase</word>
      <word>Error</word>
      <word>Event</word>
      <word>Exit</word>
      <word>For</word>
      <word>For</word>
      <word>Next</word>
      <word>Function</word>
      <word>Get</word>
      <word>GoTo</word>
      <word>If</word>
      <word>Then</word>
      <word>Else</word>
      <word>Implements</word>
      <word>Imports</word>
      <word>Inherits</word>
      <word>Interface</word>
      <word>Mid</word>
      <word>Module</word>
      <word>Namespace</word>
      <word>On</word>
      <word>Option</word>
      <word>Property</word>
      <word>RaiseEvent</word>
      <word>Randomize</word>
      <word>ReDim</word>
      <word>REM</word>
      <word>RemoveHandler</word>
      <word>Resume</word>
      <word>Return</word>
      <word>Select</word>
      <word>Case</word>
      <word>Set</word>
      <word>Stop</word>
      <word>Structure</word>
      <word>Sub</word>
      <word>SyncLock</word>
      <word>Throw</word>
      <word>Try</word>
      <word>Catch</word>
      <word>Finally</word>
      <word>While</word>
      <word>End</word>
      <word>With</word>
      <word>End</word>
    </pattern>
    <pattern name=""Property"" type=""word"">
      <font name=""Menlo"" size=""11"" style=""normal"" foreColor=""blue"" backColor=""transparent""/>
      <word>Count</word>
      <word>DateString</word>
      <word>Description</word>
      <word>Erl</word>
      <word>HelpContext</word>
      <word>HelpFile</word>
      <word>Item</word>
      <word>LastDLLError</word>
      <word>Now</word>
      <word>Number</word>
      <word>ScriptEngine</word>
      <word>ScriptEngineBuildVersion</word>
      <word>ScriptEngineMajorVersion</word>
      <word>ScriptEngineMinorVersion</word>
      <word>Source</word>
      <word>TimeOfDay</word>
      <word>Timer</word>
      <word>TimeString</word>
      <word>Today</word>
    </pattern>
    <pattern name=""Object"" type=""word"">
      <font name=""Menlo"" size=""11"" style=""normal"" foreColor=""blue"" backColor=""transparent""/>
      <word>Err</word>
    </pattern>
    <pattern name=""Method"" type=""word"">
      <font name=""Menlo"" size=""11"" style=""normal"" foreColor=""blue"" backColor=""transparent""/>
      <word>Add</word>
      <word>Clear</word>
      <word>Raise</word>
      <word>Remove</word>
    </pattern>
    <pattern name=""Function"" type=""word"">
      <font name=""Menlo"" size=""11"" style=""normal"" foreColor=""blue"" backColor=""transparent""/>
      <word>Abs</word>
      <word>AppActivate</word>
      <word>Asc</word>
      <word>AscW</word>
      <word>Atn</word>
      <word>Beep</word>
      <word>CallByName</word>
      <word>CBool</word>
      <word>CBtye</word>
      <word>CChar</word>
      <word>CDate</word>
      <word>CDbl</word>
      <word>CDec</word>
      <word>ChDir</word>
      <word>ChDrive</word>
      <word>Choose</word>
      <word>Chr</word>
      <word>ChrW</word>
      <word>CInt</word>
      <word>CLng</word>
      <word>CObj</word>
      <word>Command</word>
      <word>Conversion</word>
      <word>Cos</word>
      <word>CreateObject</word>
      <word>CShort</word>
      <word>CSng</word>
      <word>CStr</word>
      <word>CType</word>
      <word>CurDir</word>
      <word>DateAdd</word>
      <word>DateDiff</word>
      <word>DatePart</word>
      <word>DateSerial</word>
      <word>DateValue</word>
      <word>Day</word>
      <word>DDB</word>
      <word>DeleteSetting</word>
      <word>Dir</word>
      <word>Environ</word>
      <word>EOF</word>
      <word>ErrorToString</word>
      <word>Exp</word>
      <word>FileAttr</word>
      <word>FileClose</word>
      <word>FileCopy</word>
      <word>FileDateTime</word>
      <word>FileGet</word>
      <word>FileGetObject</word>
      <word>FileLen</word>
      <word>FileOpen</word>
      <word>FilePut</word>
      <word>FilePutObject</word>
      <word>FileWidth</word>
      <word>Filter</word>
      <word>Fix</word>
      <word>Format</word>
      <word>FormatCurrency</word>
      <word>FormatDateTime</word>
      <word>FormatNumber</word>
      <word>FormatPercent</word>
      <word>FreeFile</word>
      <word>FV</word>
      <word>GetAllSettings</word>
      <word>GetAttr</word>
      <word>GetChar</word>
      <word>GetException</word>
      <word>GetObject</word>
      <word>GetSetting</word>
      <word>Hex</word>
      <word>Hour</word>
      <word>IIf</word>
      <word>Input</word>
      <word>InputBox</word>
      <word>InputString</word>
      <word>InStr</word>
      <word>InStrRev</word>
      <word>Int</word>
      <word>IPmt</word>
      <word>IRR</word>
      <word>IsArray</word>
      <word>IsDate</word>
      <word>IsDBNull</word>
      <word>IsError</word>
      <word>IsNothing</word>
      <word>IsNumeric</word>
      <word>IsReference</word>
      <word>Join</word>
      <word>Kill</word>
      <word>LBound</word>
      <word>LCase</word>
      <word>Left</word>
      <word>Len</word>
      <word>LineInput</word>
      <word>Loc</word>
      <word>Lock</word>
      <word>LOF</word>
      <word>Log</word>
      <word>LSet</word>
      <word>LTrim</word>
      <word>Mid</word>
      <word>Minute</word>
      <word>MIRR</word>
      <word>MkDir</word>
      <word>Month</word>
      <word>MonthName</word>
      <word>MsgBox</word>
      <word>NPer</word>
      <word>NPV</word>
      <word>Oct</word>
      <word>Partition</word>
      <word>Pmt</word>
      <word>PPmt</word>
      <word>Print</word>
      <word>PV</word>
      <word>QBColor</word>
      <word>Rate</word>
      <word>Rename</word>
      <word>Replace</word>
      <word>Reset</word>
      <word>RGB</word>
      <word>Right</word>
      <word>RmDir</word>
      <word>Rnd</word>
      <word>Round</word>
      <word>RSet</word>
      <word>RTrim</word>
      <word>SaveSetting</word>
      <word>Second</word>
      <word>Seek</word>
      <word>SetAttr</word>
      <word>Sgn</word>
      <word>Shell</word>
      <word>Sin</word>
      <word>SLN</word>
      <word>Space</word>
      <word>SPC</word>
      <word>Split</word>
      <word>Sqr</word>
      <word>Str</word>
      <word>StrComp</word>
      <word>StrConv</word>
      <word>StrDup</word>
      <word>StrReverse</word>
      <word>Switch</word>
      <word>SYD</word>
      <word>SystemTypeName</word>
      <word>TAB</word>
      <word>Tan</word>
      <word>TimeSerial</word>
      <word>TimeValue</word>
      <word>Trim</word>
      <word>TypeName</word>
      <word>UBound</word>
      <word>UCase</word>
      <word>Unlock</word>
      <word>Val</word>
      <word>VarType</word>
      <word>VbTypeName</word>
      <word>Weekday</word>
      <word>WeekdayName</word>
      <word>Write</word>
      <word>Year</word>
    </pattern>
    <pattern name=""DataType"" type=""word"">
      <font name=""Menlo"" size=""11"" style=""normal"" foreColor=""blue"" backColor=""transparent""/>
      <word>Boolean</word>
      <word>Byte</word>
      <word>Char</word>
      <word>Date</word>
      <word>Decimal</word>
      <word>Double</word>
      <word>Integer</word>
      <word>Long</word>
      <word>Object</word>
      <word>Short</word>
      <word>Single</word>
      <word>String</word>
    </pattern>
    <pattern name=""String"" type=""block"" beginsWith=""&quot;"" endsWith=""&quot;"">
      <font name=""Menlo"" size=""11"" style=""normal"" foreColor=""#666666"" backColor=""transparent""/>
    </pattern>
    <pattern name=""Comment"" type=""block"" beginsWith=""&apos;"" endsWith=""\n"">
      <font name=""Menlo"" size=""11"" style=""normal"" foreColor=""green"" backColor=""transparent""/>
    </pattern>
    <pattern name=""Operator"" type=""word"">
      <font name=""Menlo"" size=""11"" style=""normal"" foreColor=""red"" backColor=""transparent""/>
      <word>+</word>
      <word>-</word>
      <word>=</word>
      <word>%</word>
      <word>*</word>
      <word>/</word>
      <word>AddressOf</word>
      <word>And</word>
      <word>AndAlso</word>
      <word>GetType</word>
      <word>Is</word>
      <word>Like</word>
      <word>Mod</word>
      <word>Not</word>
      <word>Or</word>
      <word>OrElse</word>
      <word>Xor</word>
    </pattern>
  </definition>
  <definition name=""XML"" caseSensitive=""false"">
    <default>
      <font name=""Menlo"" size=""11"" style=""normal"" foreColor=""black"" backColor=""transparent""/>
    </default>
    <pattern name=""MultiLineComment"" type=""block"" beginsWith=""&amp;lt;!-"" endsWith=""-&amp;gt;"">
      <font name=""Menlo"" size=""11"" style=""normal"" foreColor=""green"" backColor=""transparent""/>
    </pattern>
    <pattern name=""Markup"" type=""markup"" highlightAttributes=""true"">
      <font name=""Menlo"" size=""11"" style=""normal"" foreColor=""maroon"" backColor=""transparent"">
        <bracketStyle foreColor=""blue"" backColor=""transparent""/>
        <attributeNameStyle foreColor=""red"" backColor=""transparent""/>
        <attributeValueStyle foreColor=""blue"" backColor=""transparent""/>
      </font>
    </pattern>
  </definition>
</definitions>
            ");
        }
    }
}