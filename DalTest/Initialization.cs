/// <summary>
/// The Initialization class is used to set up the initial data for volunteers in the system.
/// It generates volunteer data, ensures unique IDs, and creates volunteer instances using predefined data.
/// </summary>

namespace DalTest;
using DalApi;
using DO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

/// <summary>
/// This class contains the logic for initializing the volunteer data.
/// </summary>

public static class Initialization
{

    //stage 1:
    /// <summary>
    /// Private static field for storing the volunteer data access layer (DAL) interface.
    /// </summary>
    //private static IVolunteer? s_dalVolunteer;

    /// <summary>
    /// Private static field for storing the call data access layer (DAL) interface.
    /// </summary>
    //private static ICall? s_dalCall;

    /// <summary>
    /// Private static field for storing the assignment data access layer (DAL) interface.
    /// </summary>
    //private static IAssignment? s_dalAssignment;

    /// <summary>
    /// Private static field for storing the configuration data access layer (DAL) interface.
    /// </summary>
    //private static IConfig? s_dalConfig;

    /// <summary>
    /// Private static field for storing a list of volunteer objects.
    /// </summary>
    //private static List<Volunteer>? VolunteersArray;

    //stage 2:

    /// <summary>
    /// Random number generator for generating random data.
    /// </summary>
    private static readonly Random s_rand = new();


    private static IDal? s_dal;



    /// <summary>
    /// Method for creating volunteer instances and populating them with predefined data.
    /// Ensures that the generated volunteer ID is unique.
    /// </summary>

    private static void createVolunteer()
    {

        //the data was written by AI
        // Array containing names of volunteers.
        string[] FullNamesVolunteers = { "John Doe", "Jane Smith", "Michael Johnson", "Emily Davis", "Chris Brown",
                    "Jessica Taylor", "David Wilson", "Sophia Moore", "Daniel Anderson", "Olivia Lee",
                    "Matthew Harris", "Lily Clark", "James Walker", "Chloe Hall", "Benjamin Young",
                    "Mia Scott", "Ethan Adams", "Isabella King", "Aiden Martinez", "Charlotte Lee",
                    "Lucas Perez", "Amelia Rodriguez", "Henry Green", "Aria Hall", "Samuel Harris",
                    "Ella Carter", "David Walker", "Avery Thompson", "Oliver Collins", "Sophie White",
                    "William Wright", "Megan Nelson", "Jack Brown", "Madison Roberts", "Jacob Carter",
                    "Sophia Brooks", "Jackson Scott", "Grace Turner", "Mason Miller", "Liam King",
                    "Charlotte Walker", "Daniel Evans", "Ella Green", "Oliver Evans", "Harper Lewis",
                    "Benjamin Lee", "Victoria Allen", "James White", "Natalie Scott", "Evan Phillips"
                    };
        // Array containing phone numbers of volunteers.
        string[] PhonesVolunteers = { "052-1234567", "053-2345678", "054-3456789", "055-4567890", "056-5678901",
                    "057-6789012", "058-7890123", "059-8901234", "050-9876543", "051-8765432",
                    "052-7654321", "053-6543210", "054-5432109", "055-4321098", "056-3210987",
                    "057-2109876", "058-1098765", "059-0987654", "050-9876543", "051-8765432",
                    "052-7654321", "053-6543210", "054-5432109", "055-4321098", "056-3210987",
                    "057-2109876", "058-1098765", "059-0987654", "050-9876543", "051-8765432",
                    "052-7654321", "053-6543210", "054-5432109", "055-4321098", "056-3210987",
                    "057-2109876", "058-1098765", "059-0987654", "050-9876543", "051-8765432",
                    "052-7654321", "053-6543210", "054-5432109", "055-4321098", "056-3210987",
                    "057-2109876", "058-1098765", "059-0987654" ,"054-0988684","052-9989654"};
        // Array containing email addresses of volunteers.
        string[] EmailsVolunteers = { "john.doe@example.com", "jane.smith@example.com", "michael.johnson@example.com",
                    "emily.davis@example.com", "chris.brown@example.com", "jessica.taylor@example.com",
                    "david.wilson@example.com", "sophia.moore@example.com", "daniel.anderson@example.com",
                    "olivia.lee@example.com", "matthew.harris@example.com", "lily.clark@example.com",
                    "james.walker@example.com", "chloe.hall@example.com", "benjamin.young@example.com",
                    "mia.scott@example.com", "ethan.adams@example.com", "isabella.king@example.com",
                    "aiden.martinez@example.com", "charlotte.lee@example.com", "lucas.perez@example.com",
                    "amelia.rodriguez@example.com", "henry.green@example.com", "aria.hall@example.com",
                    "samuel.harris@example.com", "ella.carter@example.com", "david.walker@example.com",
                    "avery.thompson@example.com", "oliver.collins@example.com", "sophie.white@example.com",
                    "william.wright@example.com", "megan.nelson@example.com", "jack.brown@example.com",
                    "madison.roberts@example.com", "jacob.carter@example.com", "sophia.brooks@example.com",
                    "jackson.scott@example.com", "grace.turner@example.com", "mason.miller@example.com",
                    "liam.king@example.com", "charlotte.walker@example.com", "daniel.evans@example.com",
                    "ella.green@example.com", "oliver.evans@example.com", "harper.lewis@example.com",
                    "benjamin.lee@example.com", "victoria.allen@example.com", "james.white@example.com",
                    "natalie.scott@example.com", "evan.phillips@example.com" };
        // Array containing passwords for volunteers.
        string[] PasswordsVolunteers = { "password123", "abc123", "qwerty123", "letmein2024", "welcome1",
                       "1234abcd", "iloveyou99", "admin123", "secretpass", "letmein22",
                       "secure2023", "testpass1", "password2024", "mysecret123", "access123",
                       "12345qwerty", "iloveme", "mypassword01", "adminpassword", "password!23",
                       "123qwerty", "letmein2023", "strongpass22", "qwertz123", "abcpassword",
                       "mypassword@2024", "passw0rd123", "iloveyou1234", "testpassword12", "openai123",
                       "adminsecure", "securepassword01", "bestpassword!", "chatrocks", "strongpass1",
                       "newpassword2024", "password01@", "welcome@123", "1234password", "secret1234",
                       "tbot2023", "securechatgpt", "pass1234", "qwertz!234", "supersecurepass",
                       "access1234", "strong2024pass", "ilovechatgpt123", "admin1password", "testpass1234" };
        // Array containing full addresses of volunteers.
        string[] FullAddressesVolunteers = {
    "Rothschild Boulevard 1, Tel Aviv",
    "King George Street 20, Tel Aviv",
    "Herzl Street 10, Tel Aviv",
    "Dizengoff Street 50, Tel Aviv",
    "Allenby Street 99, Tel Aviv",
    "Shaul HaMelech Blvd 37, Tel Aviv",
    "HaYarkon Street 100, Tel Aviv",
    "Jaffa Road 30, Jerusalem",
    "King David Street 23, Jerusalem",
    "Emek Refaim 45, Jerusalem",
    "HaChalutz 11, Jerusalem",
    "HaChoref 2, Jerusalem",
    "Ben Gurion Boulevard 12, Haifa",
    "Hertzel 48, Haifa",
    "HaNassi Boulevard 109, Haifa",
    "HaKochavim 25, Haifa",
    "HaChok 11, Haifa",
    "Herzl 15, Netanya",
    "HaDegel 77, Netanya",
    "HaAtzmaut 60, Bat Yam",
    "HaChof 3, Bat Yam",
    "HaNamal 8, Ashdod",
    "Sderot Menachem Begin 7, Ashdod",
    "Moshe Dayan Boulevard 10, Ashdod",
    "Ben Gurion 11, Ashkelon",
    "HaGiv'a 60, Ashkelon",
    "HaMashbir 14, Hadera",
    "HaSela 58, Tirat Carmel",
    "HaTzanchanim 9, Lod",
    "Herzel 90, Lod",
    "HaRishonim 11, Rishon Lezion",
    "Rothschild 100, Rishon Lezion",
    "HaHistadrut 50, Ramat Gan",
    "HaShalom Road 33, Ramat Gan",
    "Menachem Begin Blvd 48, Givatayim",
    "HaShalom Rd 6, Givatayim",
    "Begin Road 17, Petah Tikva",
    "HaTechnology 67, Petah Tikva",
    "Em Hamoshavot Rd 99, Petah Tikva",
    "HaNasi Weizman 1, Kfar Saba",
    "HaPalmach 40, Raanana",
    "Gefen 22, Be'er Sheva",
    "HaPalmach 5, Be'er Sheva",
    "HaRav Kook 5, Eilat",
    "HaBoker 45, Eilat",
    "HaGalil 3, Safed",
    "HaYam 7, Tiberias",
    "HaZionut 3, Kiryat Shmona",
    "HaHistadrut 80, Kiryat Gat",
    "HaDekel 8, Netivot"
};

        double[] LatitudeVolunteers = {
    32.0645, 32.0540, 32.0556, 32.0853, 32.0647, 32.0729, 32.0729, // Tel Aviv
    31.7780, 31.7717, 31.7780, 31.7776, 31.7817, // Jerusalem
    32.7940, 32.7940, 32.8140, 32.7900, 32.8000, // Haifa
    32.3327, 32.3337, // Netanya
    32.0276, 32.0310, // Bat Yam
    31.8034, 31.7950, 31.7940, // Ashdod (updated for "Sderot Menachem Begin 7")
    31.6690, 31.6750, // Ashkelon
    32.4346, // Hadera
    32.7090, // Tirat Carmel
    31.9517, 31.9540, // Lod (updated for "Herzel 90")
    31.9640, 31.9710, // Rishon Lezion (updated for "Rothschild 100")
    32.0600, 32.0600, // Ramat Gan
    32.0730, 32.0730, // Givatayim (updated for "HaShalom Rd 6")
    32.0820, 32.0900, 32.0950, // Petah Tikva (updated for "Em Hamoshavot Rd 99")
    32.1670, // Kfar Saba
    32.1830, // Raanana
    31.2520, 31.2540, // Be'er Sheva
    29.5580, 29.5590, // Eilat
    32.9640, // Safed
    32.7940, // Tiberias
    33.2040, // Kiryat Shmona
    31.6080, // Kiryat Gat
    31.4220  // Netivot
};

        double[] LongitudeVolunteers = {
    34.7770, 34.7650, 34.7700, 34.7818, 34.7690, 34.7815, 34.7620, // Tel Aviv
    35.2170, 35.2130, 35.2120, 35.2100, 35.2150, // Jerusalem
    34.9896, 34.9875, 34.9930, 34.9950, 34.9990, // Haifa
    34.8500, 34.8500, // Netanya
    34.7500, 34.7450, // Bat Yam
    34.6500, 34.6500, 34.6550, // Ashdod (updated for "Sderot Menachem Begin 7")
    34.5740, 34.5700, // Ashkelon
    34.9197, // Hadera
    35.0730, // Tirat Carmel
    34.8990, 34.8990, // Lod (updated for "Herzel 90")
    34.9010, 34.8000, // Rishon Lezion (updated for "Rothschild 100")
    34.8000, 34.7900, // Ramat Gan
    34.7890, 34.7810, // Givatayim (updated for "HaShalom Rd 6")
    34.7830, 34.7840, 34.7900, // Petah Tikva (updated for "Em Hamoshavot Rd 99")
    34.9000, // Kfar Saba
    34.8700, // Raanana
    34.7950, 34.7960, // Be'er Sheva
    34.9519, 34.9510, // Eilat
    35.4950, // Safed
    35.5320, // Tiberias
    35.5700, // Kiryat Shmona
    34.7600, // Kiryat Gat
    34.6180  // Netivot
};

        // Array containing roles for volunteers.
        ROLE[] RolesVolunteers = { ROLE.ADMIN, ROLE.VOLUNTEER, ROLE.VOLUNTEER, ROLE.VOLUNTEER,
    ROLE.VOLUNTEER, ROLE.VOLUNTEER, ROLE.VOLUNTEER, ROLE.VOLUNTEER, ROLE.VOLUNTEER,
    ROLE.VOLUNTEER, ROLE.VOLUNTEER, ROLE.VOLUNTEER, ROLE.VOLUNTEER, ROLE.VOLUNTEER,
    ROLE.VOLUNTEER, ROLE.VOLUNTEER, ROLE.VOLUNTEER, ROLE.VOLUNTEER, ROLE.VOLUNTEER,
    ROLE.VOLUNTEER, ROLE.VOLUNTEER, ROLE.VOLUNTEER, ROLE.VOLUNTEER, ROLE.VOLUNTEER,
    ROLE.VOLUNTEER, ROLE.VOLUNTEER, ROLE.VOLUNTEER, ROLE.VOLUNTEER, ROLE.VOLUNTEER,
    ROLE.VOLUNTEER, ROLE.VOLUNTEER, ROLE.VOLUNTEER, ROLE.VOLUNTEER, ROLE.VOLUNTEER,
    ROLE.VOLUNTEER, ROLE.VOLUNTEER, ROLE.VOLUNTEER, ROLE.VOLUNTEER, ROLE.VOLUNTEER,
    ROLE.VOLUNTEER, ROLE.VOLUNTEER, ROLE.VOLUNTEER, ROLE.VOLUNTEER, ROLE.VOLUNTEER, ROLE.VOLUNTEER,
    ROLE.VOLUNTEER, ROLE.VOLUNTEER, ROLE.VOLUNTEER, ROLE.VOLUNTEER, ROLE.VOLUNTEER};

        int i = 0;

        // Iterate over the list of full names to create volunteer instances.
        foreach (var name in FullNamesVolunteers)
        {
            int id;
            // Ensure the generated volunteer ID is unique.
            do
                id = s_rand.Next(200000000, 400000000);
            while (s_dal!.Volunteer.Read(id) != null);
            // Create a new volunteer instance with the generated and predefined data.
            string? Password = PasswordsVolunteers[i];
            string? FullAddress = FullAddressesVolunteers[i];
            ROLE Role = (i == 0) ? ROLE.ADMIN : RolesVolunteers[i];
            bool Active = (id % 2) == 0 ? true : false;
            double? MaxDistance = s_rand.Next(0, 12500);

            Volunteer newV = new Volunteer(id, FullNamesVolunteers[i], PhonesVolunteers[i], EmailsVolunteers[i],
                PasswordsVolunteers[i], FullAddressesVolunteers[i], LatitudeVolunteers[i], LongitudeVolunteers[i],
                RolesVolunteers[i], Active, MaxDistance);

            // Create the volunteer in the DAL.
            s_dal!.Volunteer.Create(newV);

            // Increment the index for the next iteration.
            i++;
        }
    }
    /// <summary>
    /// This method generates arrays representing the call type, call descriptions, full addresses, coordinates (latitude and longitude),
    /// as well as timestamps for the creation and expected completion of each call.
    /// The method sets up a list of calls that can be used for further processing or display in an application handling vehicle service calls.    /// </summary>
    private static void createCall()
    {
        // Array of different call types
        TYPEOFCALL[] TypesOfCalls ={
    TYPEOFCALL.FLATTIRE, TYPEOFCALL.REDRIVE, TYPEOFCALL.CARBURGLARY, TYPEOFCALL.FLATTIRE, TYPEOFCALL.REDRIVE,
    TYPEOFCALL.CARBURGLARY, TYPEOFCALL.REDRIVE, TYPEOFCALL.CARBURGLARY, TYPEOFCALL.FLATTIRE, TYPEOFCALL.CARBURGLARY,
    TYPEOFCALL.FLATTIRE, TYPEOFCALL.REDRIVE, TYPEOFCALL.CARBURGLARY, TYPEOFCALL.FLATTIRE, TYPEOFCALL.REDRIVE,
    TYPEOFCALL.CARBURGLARY, TYPEOFCALL.FLATTIRE, TYPEOFCALL.CARBURGLARY, TYPEOFCALL.REDRIVE, TYPEOFCALL.FLATTIRE,
    TYPEOFCALL.REDRIVE, TYPEOFCALL.CARBURGLARY, TYPEOFCALL.FLATTIRE, TYPEOFCALL.REDRIVE, TYPEOFCALL.CARBURGLARY,
    TYPEOFCALL.FLATTIRE, TYPEOFCALL.REDRIVE, TYPEOFCALL.CARBURGLARY, TYPEOFCALL.FLATTIRE, TYPEOFCALL.REDRIVE,
    TYPEOFCALL.CARBURGLARY, TYPEOFCALL.REDRIVE, TYPEOFCALL.FLATTIRE, TYPEOFCALL.CARBURGLARY, TYPEOFCALL.REDRIVE,
    TYPEOFCALL.FLATTIRE, TYPEOFCALL.CARBURGLARY, TYPEOFCALL.REDRIVE, TYPEOFCALL.FLATTIRE, TYPEOFCALL.CARBURGLARY,
    TYPEOFCALL.REDRIVE, TYPEOFCALL.FLATTIRE, TYPEOFCALL.REDRIVE, TYPEOFCALL.CARBURGLARY, TYPEOFCALL.FLATTIRE,
    TYPEOFCALL.CARBURGLARY, TYPEOFCALL.REDRIVE, TYPEOFCALL.FLATTIRE,TYPEOFCALL.REDRIVE, TYPEOFCALL.FLATTIRE};
        // Array of call descriptions
        string[] callDescriptions =
{
    "A man in his 40s is stuck at the side of the road with a serious flat tire due to a nail puncturing it.",
    "A woman in her 30s is calling for help because her car won't start, she thinks it's due to the battery.",
    "A man in his 50s reports a car break-in. The car was stolen from a shopping mall parking lot. He needs help securing the vehicle.",
    "A young man in his 20s reports that his car won't start due to an engine problem. He's stuck on a main road.",
    "A woman in her 40s calls for help because her car won't start, probably due to a dead battery. She is stuck in a parking lot.",
    "A man in his 60s is stuck at the side of a rural road with a flat tire. He can't change it himself.",
    "A young man in his 20s reports a car break-in that happened while the car was parked near his friend's house. He needs help securing the vehicle.",
    "A woman in her 30s calls for help because her car won't start, most likely due to a dead battery. She's stuck on a busy street.",
    "A man in his 50s reports a flat tire at the side of the road. He can't change the tire by himself and needs immediate assistance.",
    "A woman in her 20s calls for help after discovering that her car won't start. She suspects the battery is dead.",
    "A man in his 30s calls for help because his car won't start. It seems like a battery issue.",
    "A woman in her 50s calls for help after her car unexpectedly stops working. She's stuck on the highway and needs immediate help.",
    "A young man in his 20s reports that his car won't start due to an engine problem. He's stuck on a busy road and requests urgent assistance.",
    "A man in his 40s calls for help because his car's battery is faulty and won't start. He's stuck on a side street.",
    "A woman in her 60s calls for help after discovering that her car won't start. She thinks there's an issue with the electrical system.",
    "A young man in his 20s reports an engine issue with his car. He’s stuck on a fast lane and asks for immediate help.",
    "A man in his 30s calls for help because his car won't start, possibly due to an ignition system issue.",
    "A woman in her 40s calls for help after discovering that her car won’t start due to a fuel system problem.",
    "A man in his 50s calls for help because he can't change a flat tire on the front of his car. He's stuck on the side of the road.",
    "A young man in his 20s calls for help because his car won’t start, probably due to an electrical system issue.",
    "A man in his 40s reports a car break-in while it was parked in a mall parking lot. He needs help securing the vehicle.",
    "A woman in her 30s needs help replacing a flat tire. She can't change it and is stuck on a main road.",
    "A man in his 60s calls for help after discovering that his car was stolen from in front of his house. He needs assistance securing the car.",
    "A young man in his 20s reports a dead battery in his car. He can't start the car and needs immediate help.",
    "A man in his 30s calls for help because his car won’t start, probably due to an electrical system issue.",
    "A woman in her 40s needs help starting her car after the battery died. She is stuck on a side street.",
    "A man in his 20s calls for help because his car won’t start due to a dead battery. He's stuck at a gas station.",
    "A woman in her 30s calls for help after discovering her car won’t start. She thinks the battery is drained.",
    "A man in his 50s calls for help after his car was stolen. He needs assistance securing the vehicle.",
    "A young man in his 30s reports a flat tire at the side of the road. He can't change the tire and needs immediate assistance.",
    "A man in his 40s calls for help after his car's battery died. He needs immediate help to start the car.",
    "A woman in her 50s calls for help after discovering that her car won’t start. She needs help starting it.",
    "A young man in his 20s calls for help after discovering his car’s battery died. He can't start the car.",
    "A man in his 30s calls for help because his car won’t start. The engine is not turning on, and he needs urgent assistance.",
    "A woman in her 40s calls for help after realizing her car won’t start. She's stuck on a main road.",
    "A man in his 30s reports that his car won't start due to a fault with the ignition system. He’s stuck on the side of the road.",
    "A woman in her 40s calls for help after her car suffered an issue with the drive system. She can't start the car.",
    "A young man in his 20s reports a fuel system issue with his car. It won’t start, and he needs assistance.",
    "A man in his 50s calls for help after his car's fuel line burst. It won't start, and he needs immediate help.",
    "A woman in her 30s reports a dead battery in her car. She can't start the car and needs help starting it.",
    "A man in his 40s reports that his car won't start due to an issue with the ignition system. He's stuck on the side of the road.",
    "A young man in his 20s calls for help because his car's clutch system is malfunctioning. He needs urgent assistance.",
    "A man in his 50s calls for help after his car won't start due to an issue with the electrical system.",
    "A woman in her 60s needs help starting her car after the battery died.",
    "A man in his 30s calls for help after discovering an issue with the car's alignment. He can’t start the car.",
    "A woman in his 40s reports that his car won't start due to an issue with the ignition system. He's stuck on the side of the road.",
    "A old man in his 20s calls for help because his car's clutch system is malfunctioning. He needs urgent assistance.",
    "A old woman in his 50s calls for help after his car won't start due to an issue with the electrical system.",
    "A woman with her husband in her 60s needs help starting her car after the battery died.",
    "A child in his 30s calls for help after discovering an issue with the car's alignment. He can’t start the car."

};
        // Array of full addresses for each call
        string[] FullAddressesOfCalls = {
    "Jabotinsky 45, Ramat Gan",
    "Rothschild Blvd 29, Tel Aviv",
    "Derech HaMakabim 14, Moshav Maor, Menashe", // Specific address in Moshav Maor
    "Ein Kerem 21, Jerusalem",
    "Kiryat Yovel 35, Jerusalem",
    "Ramat Eshkol 10, Jerusalem",
    "Hadar 12, Haifa", // Hadar HaCarmel neighborhood
    "Yigal Alon 56, Tel Aviv",
    "Neve Tzedek 7, Tel Aviv",
    "Herzl 9, Be'er Sheva",
    "Shalom 10, Netanya", // "Shalom Street" in Netanya
    "Dizengoff 70, Tel Aviv",
    "Bialik 18, Herzliya",
    "Kiryat Moshe 2, Jerusalem",
    "Herzl 4, Rehovot", // "Herzl Street" in Rehovot
    "HaNasi 25, Ashkelon", // "HaNasi Street" in Ashkelon
    "Jaffa 23, Tel Aviv",
    "HaAlon 48, Eilat", // "HaAlon Street" in Eilat
    "HaTzabar 11, Ramat Gan", // "HaTzabar Street" in Ramat Gan
    "Shuk HaCarmel 3, Tel Aviv",
    "Maayan 25, Raanana",
    "Zichron Yaakov 12, Zichron Yaakov", // Corrected to Zichron Yaakov city
    "Acre Road 55, Acre", // Main road in Acre
    "HaShita 34, Nesher", // "HaShita Street" in Nesher city
    "Ramat Aviv 15, Tel Aviv",
    "HaCarmel 9, Haifa", // "HaCarmel Street" in Haifa
    "HaTe'ena 5, Rehovot", // "HaTe'ena Street" in Rehovot
    "Menachem Begin Blvd 100, Tel Aviv",
    "Ramat Shlomo 6, Jerusalem",
    "Bar Ilan 21, Ramat Gan",
    "Yarkon Park, Tel Aviv", // Yarkon Park, no specific number
    "HaBanim 4, Binyamina", // "HaBanim Street" in Binyamina
    "HaArava 18, Eilat", // "HaArava Street" in Eilat
    "HaDagan 22, Moshav Sde Warburg, Sharon", // Specific address in Sde Warburg
    "Kikar Hamedina 12, Tel Aviv",
    "HaShaked 8, Ramat Gan",
    "HaSharon 30, Kfar Saba", // "HaSharon Street" in Kfar Saba
    "HaGolan 12, Ashdod",
    "Oded 45, Petah Tikva",
    "Tchernichovsky 30, Tel Aviv",
    "Lev Ha'Ir 19, Tel Aviv",
    "HaZohar 14, Herzliya", // "HaZohar Street" in Herzliya
    "Herzliya Pituach 6, Herzliya",
    "HaNegev 24, Be'er Sheva",
    "HaShizaf 11, Rehovot", // "HaShizaf Street" in Rehovot
    "Zanvil 3, Petah Tikva",
    "Jabotinsky 40, Ramat Gan", // Duplicate, kept as a separate call
    "Sderot 12, Kiryat Gat",
    "Shalom Aleichem 4, Rishon Lezion",
    "HaSadot 30, Ashkelon" // "HaSadot Street" in Ashkelon
};        // Array of latitude coordinates for each call
        double[] LatitudeOfCall ={
    32.0722,
    32.0716,
    32.4168,
    31.7686,
    31.7601,
    31.7997,
    32.7937,
    32.0620,
    32.0601,
    31.2514,
    32.3276,
    32.0831,
    32.1643,
    31.7936,
    31.8920,
    31.6660,
    32.0621,
    29.5606,
    32.0726,
    32.0683,
    32.1852,
    32.5599,
    32.9238,
    32.7667,
    32.1009,
    32.8126,
    31.8906,
    32.0700,
    31.8105,
    32.0709,
    32.0910,
    32.5312,
    29.5492,
    32.2227,
    32.0818,
    32.0754,
    32.1764,
    31.7981,
    32.0836,
    32.0784,
    32.0710,
    32.1718,
    32.1648,
    31.2464,
    31.8950,
    32.0967,
    32.0722,
    31.6033,
    31.9680,
    31.6702
};

        double[] LongitudeOfCall = {
    34.8052,
    34.7738,
    35.0355,
    35.1763,
    35.1770,
    35.2227,
    35.0068,
    34.7937,
    34.7645,
    34.7997,
    34.8569,
    34.7770,
    34.8437,
    35.1950,
    34.8090,
    34.5683,
    34.7629,
    34.9452,
    34.8057,
    34.7663,
    34.8789,
    34.9351,
    35.0772,
    35.0483,
    34.7891,
    34.9922,
    34.8098,
    34.7909,
    35.2285,
    34.8105,
    34.7820,
    34.9200,
    34.9547,
    34.8966,
    34.7831,
    34.8037,
    34.9248,
    34.6534,
    34.9125,
    34.7850,
    34.7709,
    34.8690,
    34.8041,
    34.7981,
    34.8085,
    34.9255,
    34.8052,
    34.7675,
    34.7915,
    34.5800
};
        // Array of timestamps representing the creation time of each call
        DateTime[] OpenTimeOfCalls = {
    new DateTime(2025, 06, 25, 14, 32, 10),
    new DateTime(2025, 06, 18, 09, 47, 35),
    new DateTime(2025, 06, 11, 16, 15, 42),
    new DateTime(2025, 06, 04, 11, 08, 23),
    new DateTime(2025, 05, 28, 17, 29, 05),
    new DateTime(2025, 05, 21, 08, 55, 12),
    new DateTime(2025, 05, 14, 12, 40, 51),
    new DateTime(2025, 05, 07, 15, 18, 44),
    new DateTime(2025, 04, 30, 07, 22, 19),
    new DateTime(2025, 04, 23, 19, 33, 37),
    new DateTime(2025, 04, 16, 06, 15, 00),
    new DateTime(2025, 04, 09, 13, 12, 09),
    new DateTime(2025, 04, 02, 10, 44, 55),
    new DateTime(2025, 03, 26, 18, 03, 28),
    new DateTime(2025, 03, 19, 14, 25, 13),
    new DateTime(2025, 03, 12, 09, 38, 47),
    new DateTime(2025, 03, 05, 11, 10, 59),
    new DateTime(2025, 02, 26, 08, 48, 33),
    new DateTime(2025, 02, 19, 12, 27, 41),
    new DateTime(2025, 02, 12, 17, 45, 18),
    new DateTime(2025, 02, 05, 10, 53, 26),
    new DateTime(2025, 01, 29, 07, 15, 32),
    new DateTime(2025, 01, 22, 13, 44, 09),
    new DateTime(2025, 01, 15, 16, 11, 00),
    new DateTime(2025, 01, 08, 09, 59, 05),
    new DateTime(2025, 01, 01, 11, 35, 47),
    new DateTime(2024, 12, 25, 08, 20, 31),
    new DateTime(2024, 12, 18, 15, 25, 13),
    new DateTime(2024, 12, 11, 14, 44, 22),
    new DateTime(2024, 12, 04, 07, 53, 39),
    new DateTime(2024, 11, 27, 10, 32, 14),
    new DateTime(2024, 11, 20, 17, 18, 59),
    new DateTime(2024, 11, 13, 12, 04, 45),
    new DateTime(2024, 11, 06, 13, 59, 06),
    new DateTime(2024, 10, 30, 06, 35, 11),
    new DateTime(2024, 10, 23, 16, 13, 03),
    new DateTime(2024, 10, 16, 09, 42, 27),
    new DateTime(2024, 10, 09, 18, 00, 00),
    new DateTime(2024, 10, 02, 11, 12, 38),
    new DateTime(2024, 09, 25, 08, 29, 21),
    new DateTime(2024, 09, 18, 14, 57, 12),
    new DateTime(2024, 09, 11, 13, 13, 45),
    new DateTime(2024, 09, 04, 10, 36, 53),
    new DateTime(2024, 08, 28, 15, 09, 34),
    new DateTime(2024, 08, 21, 07, 44, 16),
    new DateTime(2024, 08, 14, 12, 00, 00),
    new DateTime(2024, 08, 07, 17, 29, 48),
    new DateTime(2024, 07, 31, 16, 11, 29),
    new DateTime(2024, 07, 24, 13, 33, 08),
    new DateTime(2024, 07, 17, 09, 26, 40),
    new DateTime(2024, 07, 10, 08, 55, 52)
};

        // Array of timestamps representing the expected completion time of each call
        DateTime?[] MaxTimeToFinishOfCalls ={
    null,
    null,
    null,
    null,
    null,
    null,
    new DateTime(2025, 05, 16, 18, 40, 51),
    new DateTime(2025, 05, 08, 17, 18, 44),
    new DateTime(2025, 05, 01, 08, 22, 19),
    new DateTime(2025, 04, 24, 22, 33, 37),
    new DateTime(2025, 04, 18, 09, 15, 00),
    new DateTime(2025, 04, 10, 23, 12, 09),
    new DateTime(2025, 04, 04, 16, 44, 55),
    new DateTime(2025, 03, 28, 21, 03, 28),
    new DateTime(2025, 03, 21, 23, 25, 13),
    new DateTime(2025, 03, 13, 19, 38, 47),
    new DateTime(2025, 03, 06, 14, 10, 59),
    new DateTime(2025, 02, 27, 19, 48, 33),
    new DateTime(2025, 02, 20, 19, 27, 41),
    new DateTime(2025, 02, 14, 20, 45, 18),
    new DateTime(2025, 02, 06, 16, 53, 26),
    new DateTime(2025, 01, 30, 13, 15, 32),
    new DateTime(2025, 01, 24, 23, 44, 09),
    new DateTime(2025, 01, 16, 20, 11, 00),
    new DateTime(2025, 01, 09, 18, 59, 05),
    new DateTime(2025, 01, 02, 17, 35, 47),
    new DateTime(2024, 12, 26, 13, 20, 31),
    new DateTime(2024, 12, 20, 01, 25, 13),
    new DateTime(2024, 12, 12, 21, 44, 22),
    new DateTime(2024, 12, 06, 08, 53, 39),
    new DateTime(2024, 11, 29, 19, 32, 14),
    new DateTime(2024, 11, 21, 18, 18, 59),
    new DateTime(2024, 11, 14, 15, 04, 45),
    new DateTime(2024, 11, 08, 14, 59, 06),
    new DateTime(2024, 11, 01, 07, 35, 11),
    new DateTime(2024, 10, 24, 22, 13, 03),
    new DateTime(2024, 10, 17, 19, 42, 27),
    new DateTime(2024, 10, 10, 19, 00, 00),
    new DateTime(2024, 10, 03, 18, 12, 38),
    new DateTime(2024, 09, 26, 19, 29, 21),
    new DateTime(2024, 09, 20, 17, 57, 12),
    new DateTime(2024, 09, 12, 22, 13, 45),
    new DateTime(2024, 09, 05, 20, 36, 53),
    new DateTime(2024, 08, 30, 21, 09, 34),
    new DateTime(2024, 08, 23, 10, 44, 16),
    new DateTime(2024, 08, 15, 19, 00, 00),
    new DateTime(2024, 08, 08, 23, 29, 48),
    new DateTime(2024, 08, 01, 17, 11, 29),
    new DateTime(2024, 07, 26, 18, 33, 08),
    new DateTime(2024, 07, 18, 21, 26, 40),
    new DateTime(2024, 07, 11, 18, 55, 52)
};


        for (int i = 0; i < callDescriptions.Length; i++)
        {
            Call newC = new Call(
                0,
                TypesOfCalls[i],
                callDescriptions[i],
                FullAddressesOfCalls[i],
                LatitudeOfCall[i],
                LongitudeOfCall[i],
                OpenTimeOfCalls[i],
                MaxTimeToFinishOfCalls[i]
            );
            s_dal!.Call.Create(newC);
        }


    }
    /// <summary>
    /// This method reads all the call records and volunteer records from the data access layer.
    /// It uses predefined arrays of treatment statuses, entry times, and end times to create `Assignment` objects.
    /// Each assignment is created by pairing data from the call and volunteer lists at the same index and is then saved to the database.
    /// </summary>
    private static void createAssignment()
    {
        // Retrieve all call and volunteer records from the data access layer
        List<Call> CallIds = (List<Call?>)s_dal!.Call.ReadAll().ToList();
        List<Volunteer> VolunteerIds = (List<Volunteer?>)s_dal!.Volunteer.ReadAll().ToList();
        // Array of predefined treatment statuses for each assignment
        TYPEOFTREATMENT?[] treatmentStatuses ={
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            TYPEOFTREATMENT.TREATE,
            TYPEOFTREATMENT.SELFCANCELLATION,
            TYPEOFTREATMENT.CANCELINGANADMINISTRATOR,
            TYPEOFTREATMENT.TREATE,
            TYPEOFTREATMENT.TREATE,
            TYPEOFTREATMENT.SELFCANCELLATION,
            TYPEOFTREATMENT.TREATE,
            TYPEOFTREATMENT.TREATE,
            TYPEOFTREATMENT.CANCELINGANADMINISTRATOR,
            TYPEOFTREATMENT.TREATE,
            TYPEOFTREATMENT.TREATE,
            TYPEOFTREATMENT.CANCELINGANADMINISTRATOR,
            TYPEOFTREATMENT.SELFCANCELLATION,
            TYPEOFTREATMENT.TREATE,
            TYPEOFTREATMENT.TREATE,
            TYPEOFTREATMENT.TREATE,
            TYPEOFTREATMENT.CANCELINGANADMINISTRATOR,
            TYPEOFTREATMENT.CANCELINGANADMINISTRATOR,
            TYPEOFTREATMENT.TREATE,
            TYPEOFTREATMENT.TREATE,
            TYPEOFTREATMENT.TREATE,
            TYPEOFTREATMENT.SELFCANCELLATION,
            TYPEOFTREATMENT.TREATE,
            TYPEOFTREATMENT.TREATE,
            TYPEOFTREATMENT.TREATE,
            TYPEOFTREATMENT.SELFCANCELLATION,
            TYPEOFTREATMENT.CANCELINGANADMINISTRATOR,
            TYPEOFTREATMENT.TREATE,
            TYPEOFTREATMENT.TREATE,
            TYPEOFTREATMENT.CANCELLATIONHASEXPIRED,
            TYPEOFTREATMENT.CANCELLATIONHASEXPIRED,
            TYPEOFTREATMENT.CANCELLATIONHASEXPIRED,
            TYPEOFTREATMENT.CANCELLATIONHASEXPIRED,
            TYPEOFTREATMENT.CANCELLATIONHASEXPIRED,
            TYPEOFTREATMENT.CANCELLATIONHASEXPIRED,
            TYPEOFTREATMENT.CANCELLATIONHASEXPIRED
        };
        // Array of entry times for treatment assignments
        DateTime[] ArrayOfEntryTimeForTreatment ={
    new DateTime(2024, 01, 01, 00, 51, 54),
    new DateTime(2023, 11, 22, 19, 35, 35),
    new DateTime(2023, 10, 15, 11, 07, 41),
    new DateTime(2023, 12, 06, 01, 45, 01),
    new DateTime(2023, 09, 19, 10, 17, 45),
    new DateTime(2023, 12, 03, 17, 14, 02),
    new DateTime(2023, 08, 10, 14, 36, 56),
    new DateTime(2024, 01, 23, 20, 42, 53),
    new DateTime(2023, 11, 09, 11, 17, 25),
    new DateTime(2023, 07, 23, 04, 48, 53),
    new DateTime(2024, 02, 14, 11, 58, 07),
    new DateTime(2023, 09, 29, 22, 15, 33),
    new DateTime(2024, 03, 06, 04, 36, 17),
    new DateTime(2023, 11, 11, 04, 02, 48),
    new DateTime(2023, 12, 16, 18, 44, 53),
    new DateTime(2024, 01, 02, 14, 24, 00),
    new DateTime(2023, 10, 01, 22, 58, 33),
    new DateTime(2023, 12, 24, 13, 19, 03),
    new DateTime(2024, 01, 17, 07, 34, 38),
    new DateTime(2023, 07, 19, 20, 01, 40),
    new DateTime(2023, 09, 27, 10, 21, 30),
    new DateTime(2024, 02, 11, 17, 01, 21),
    new DateTime(2023, 11, 14, 13, 25, 10),
    new DateTime(2023, 08, 26, 05, 39, 24),
    new DateTime(2024, 03, 20, 04, 42, 53),
    new DateTime(2023, 10, 12, 07, 15, 31),
    new DateTime(2023, 09, 17, 21, 31, 07),
    new DateTime(2023, 12, 21, 20, 09, 03),
    new DateTime(2023, 07, 15, 17, 02, 28),
    new DateTime(2024, 01, 30, 05, 38, 44),
    new DateTime(2023, 11, 05, 20, 53, 41),
    new DateTime(2023, 10, 18, 13, 35, 25),
    new DateTime(2023, 09, 23, 18, 02, 06),
    new DateTime(2024, 01, 13, 11, 43, 12),
    new DateTime(2023, 08, 18, 22, 32, 19),
    new DateTime(2024, 02, 02, 03, 34, 11),
    new DateTime(2023, 11, 30, 08, 51, 00),
    new DateTime(2023, 07, 08, 23, 56, 26),
    new DateTime(2023, 09, 12, 06, 16, 35),
    new DateTime(2024, 03, 11, 04, 58, 08),
    new DateTime(2023, 10, 21, 00, 32, 53),
    new DateTime(2023, 08, 12, 09, 18, 18),
    new DateTime(2023, 12, 13, 17, 36, 35),
    new DateTime(2023, 11, 17, 07, 14, 27),
    new DateTime(2024, 01, 25, 13, 32, 47),
    new DateTime(2023, 09, 30, 12, 47, 37),
    new DateTime(2023, 08, 30, 22, 14, 48),
    new DateTime(2024, 03, 07, 19, 37, 52),
    new DateTime(2023, 07, 05, 16, 41, 02),
    new DateTime(2023, 12, 08, 20, 14, 08)
};
        // Array of end times for treatment assignments
        DateTime?[] ArrayOfEndTimeOfTreatment ={
    null,
    null,
    null,
    null,
    null,
    null,
    null,
    null,
    null,
    null,
    null,
    null,
    null,
    null,
    null,
    new DateTime(2026, 11, 11, 07, 55, 22),
    new DateTime(2026, 12, 16, 20, 30, 05),
    new DateTime(2026, 01, 02, 17, 10, 03),
    new DateTime(2026, 10, 01, 03, 55, 59),
    new DateTime(2026, 12, 24, 15, 55, 35),
    new DateTime(2026, 01, 17, 10, 05, 47),
    new DateTime(2026, 07, 19, 19, 30, 25),
    new DateTime(2026, 09, 27, 13, 40, 55),
    new DateTime(2026, 02, 11, 21, 15, 42),
    new DateTime(2026, 11, 14, 17, 42, 20),
    new DateTime(2026, 08, 25, 03, 46, 04),
    new DateTime(2026, 03, 20, 06, 50, 50),
    new DateTime(2026, 10, 12, 09, 58, 12),
    new DateTime(2026, 09, 17, 00, 57, 01),
    new DateTime(2026, 12, 21, 22, 35, 44),
    new DateTime(2026, 07, 15, 16, 48, 22),
    new DateTime(2026, 01, 30, 08, 55, 59),
    new DateTime(2026, 11, 05, 22, 30, 35),
    new DateTime(2026, 10, 18, 17, 40, 19),
    new DateTime(2026, 09, 23, 19, 25, 05),
    new DateTime(2026, 01, 13, 15, 05, 37),
    new DateTime(2026, 08, 18, 23, 55, 21),
    new DateTime(2026, 02, 01, 04, 30, 50),
    new DateTime(2026, 11, 30, 12, 15, 33),
    new DateTime(2026, 07, 08, 01, 38, 15),
    new DateTime(2026, 09, 12, 09, 52, 40),
    new DateTime(2026, 03, 11, 07, 35, 10),
    new DateTime(2026, 10, 20, 04, 05, 42),
    new DateTime(2026, 08, 12, 12, 30, 02),
    new DateTime(2026, 12, 13, 16, 55, 55),
    null,
    null,
    null,
    null,
    null,
    null,
    null,
    };

        int i = 0;
        // Iterate through the list of call records to create new assignments
        foreach (var call in CallIds)

        {
            // Create a new Assignment object using data from the call, volunteer, entry and end times, and treatment status
            Assignment newA = new Assignment(0, CallIds[i].Id, VolunteerIds[i].Id,
            ArrayOfEntryTimeForTreatment[i], ArrayOfEndTimeOfTreatment[i],
            treatmentStatuses[i]);

            // Save the new assignment to the data access layer
            s_dal!.Assignment.Create(newA);
            i++;
        }
    }


    /// <summary>
    /// This function accepts four data access layer (DAL) interfaces for volunteers, calls, assignments, and configuration.
    /// It performs checks to ensure that the provided DAL objects are not null. If any of them are null, a `NullReferenceException` is thrown.
    /// After that, the function resets the configuration values and deletes all records from the volunteer, call, and assignment lists.
    /// Finally, it calls methods to create new lists of volunteers, calls, and assignments.
    /// </summary>
    //public static void Do(IDal dal)//stage 2
    public static void Do()
    {
        //s_dalVolunteer = dalVolunteer ?? throw new NullReferenceException("DAL object can not be null!");//stage 1
        //s_dalCall = dalCall ?? throw new NullReferenceException("DAL object can not be null!");//stage 1
        //s_dalAssignment = dalAssignment ?? throw new NullReferenceException("DAL object can not be null!");//stage 1
        //s_dalConfig = dalConfig ?? throw new NullReferenceException("DAL object can not be null!");//stage 1
        //s_dal = dal ?? throw new NullReferenceException("DAL object can not be null!"); // stage 2
        s_dal = DalApi.Factory.Get; //stage 4
        Console.WriteLine("Reset Configuration values and List values...");

        //s_dalConfig.Reset();//stage 1
        //s_dalVolunteer.DeleteAll();//stage 1
        //s_dalCall.DeleteAll();//stage 1
        //s_dalAssignment.DeleteAll();//stage 1

        //s_dal.ResetDB();//stage 2

        Console.WriteLine("Initializing Students list ...");
        createVolunteer();
        Console.WriteLine("I am done vol");
        createCall();
        Console.WriteLine("I am done call");
        createAssignment();
        Console.WriteLine("I am done");

    }




}




