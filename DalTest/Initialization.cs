/// <summary>
/// The Initialization class is used to set up the initial data for volunteers in the system.
/// It generates volunteer data, ensures unique IDs, and creates volunteer instances using predefined data.
/// </summary>

namespace DalTest;
using DalApi;
using Dal;
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
    "Rothschild Boulevard, Tel Aviv",
    "King George St, Tel Aviv-Yafo, Israel",
    "Herzl St, Tel Aviv-Jaffa",
    "Dizengoff St, Tel Aviv-Yafo, Israel",
    "Allenby St, Tel Aviv-Yafo, Israel",
    "Shaul Hamelekh, Tel Aviv, Israel",
    "Hayarkon St, Tel Aviv-Yafo, Israel",
    "Yafo, Jerusalem, Israel",
    "King David St, Jerusalem, Israel",
    "Emek Refaim, Jerusalem, Israel",
    "Weizman St, Ra'Anana, Israel",
    "Ezer Weizman St, Hod Hasharon, Israel",
    "Sderot Ben Gurion, Haifa, Israel",
    "Herzl St, Haifa, Israel",
    "Hanassi Blvd, Haifa, Israel",
    "Yerushalayim St, Haifa, Israel",
    "Ma'Abarot, Haifa, Israel",
    "Herzl, Netanya, Israel",
    "Hadegel St, Tel Aviv-Yafo, Israel",
    "Ha'Atsmaut Blvd, Bat Yam, Israel",
    "Ha-Rav Nisanbaum St, Bat Yam, Israel",
    "Hanamal, Ashdod, Israel",
    "Sderot Menachem Begin, Ashdod, Israel",
    "Moshe Dayan Avenue, Ashdod, Israel",
    "David Ben Gurion Avenue, Ashkelon, Israel",
    "Arye Tagar St, Ashkelon, Israel",
    "Sderot Rothschild, Hadera, Israel",
    "Hasela St, Atlit, Israel",
    "Herzl St, Lod, Israel",
    "Dani Mass St, Lod, Israel",
    "David Remez St, Or Yehuda, Israel",
    "Hagdud Haivri St, Ra'anana, Israel",
    "Haahot Haya Oster St Ramat Gan, Ramat Gan, Israel",
    "Ahuza St, Ra'Anana, Israel",
    "Keren Hayesod St, Ra'Anana, Israel",
    "Derech Jaffa, Tel Aviv-Yafo, Israel",
    "Menachem Begin, Petaẖ Tiqwa, Israel",
    "Hamalbim, Jerusalem, Israel",
    "Em Hamoshavot Rd, Petah Tikva, Israel",
    "Weizmann St, Kfar Saba, Israel",
    "Ha-Palmakh St, Ra'Anana, Israel",
    "Haenergia St, Be'Er Sheva, Israel",
    "Ha-Palmakh St, Be'Er Sheva, Israel",
    "Harav Kook St, Hertsliya, Israel",
    "Yehuda Halevi St, Hertsliya, Israel",
    "Ha-Galil St, Ra'Anana, Israel",
    "Hayam, Tiberias, Israel",
    "Tel Hai St, Qiryat Shemona, Israel",
    "Sderot Haatsma'Ut, Kiryat Gat, Israel",
    "HaDekel, Netivot, Israel"
};

    

        double[] LatitudeVolunteers = {
    32.0688,   // Rothschild Boulevard 1, Tel Aviv
    32.0667,   // 20 King George St, Tel Aviv-Yafo
    32.0615,   // Herzl St 10, Tel Aviv-Jaffa
    32.0853,   // 50 Dizengoff St, Tel Aviv-Yafo
    32.0733,   // 99 Allenby St, Tel Aviv-Yafo
    32.0551,   // Shaul Hamelekh, Tel Aviv
    32.0850,   // 99 Hayarkon St, Tel Aviv-Yafo
    31.7683,   // Yafo, Jerusalem
    31.7774,   // 23 King David St, Jerusalem
    31.7554,   // 40 Emek Refaim, Jerusalem
    32.1913,   // 2 Weizman St, Ra'Anana
    32.1654,   // 7 Ezer Weizman St, Hod Hasharon
    32.7940,   // 12 Sderot Ben Gurion, Haifa
    32.7946,   // 24 Herzl St, Haifa
    32.8054,   // 109 Hanassi Blvd, Haifa
    32.8090,   // 29 Yerushalayim St, Haifa
    32.8223,   // 7 Ma'Abarot, Haifa
    32.3370,   // 28 Herzl, Netanya
    32.0794,   // 8 Hadegel St, Tel Aviv-Yafo
    32.0224,   // 36 Ha'Atsmaut Blvd, Bat Yam
    32.0330,   // 11 Ha-Rav Nisanbaum St, Bat Yam
    31.8011,   // Hanamal, Ashdod
    31.8010,   // Sderot Menachem Begin, Ashdod
    31.8030,   // Moshe Dayan Avenue, Ashdod
    31.6672,   // 1 David Ben Gurion Avenue, Ashkelon
    31.6685,   // 1 Arye Tagar St, Ashkelon
    32.4332,   // 40 Sderot Rothschild, Hadera
    32.6374,   // 26 Hasela St, Atlit
    31.9515,   // 21 Herzl St, Lod
    31.9511,   // 2 Dani Mass St, Lod
    31.9615,   // 2 David Remez St, Or Yehuda
    32.1850,   // 9 Hagdud Haivri St, Ra'anana
    32.0685,   // 26 Haahot Haya Oster St 2, Ramat Gan
    32.1830,   // 95 Ahuza St, Ra'anana
    32.1797,   // 5 Keren Hayesod St, Ra'anana
    32.0628,   // 6 Derech Jaffa, Tel Aviv-Yafo
    32.0910,   // 1 Menachem Begin, Petah Tikva
    31.7712,   // 1 Hamalbim, Jerusalem
    32.0955,   // 94 Em Hamoshavot Rd, Petah Tikva
    32.1665,   // 157 Weizmann St, Kfar Saba
    32.1915,   // 13 Ha-Palmakh St, Ra'anana
    31.2520,   // 77 Haenergia St, Be'er Sheva
    31.2571,   // 5 Ha-Palmakh St, Be'er Sheva
    32.1653,   // 66 Harav Kook St, Herzliya
    32.1668,   // 14 Yehuda Halevi St, Herzliya
    32.1918,   // 43 Ha-Galil St, Ra'anana
    32.7921,   // Hayam, Tiberias
    33.2851,   // 106 Tel Hai St, Qiryat Shemona
    31.6120,   // 64 Sderot Haatsma'Ut, Kiryat Gat
    31.4010    // 2 HaDekel, Netivot
};

        double[] LongitudeVolunteers = {
    34.7818,   // Rothschild Boulevard 1, Tel Aviv
    34.7730,   // 20 King George St, Tel Aviv-Yafo
    34.7715,   // Herzl St 10, Tel Aviv-Jaffa
    34.7758,   // 50 Dizengoff St, Tel Aviv-Yafo
    34.7695,   // 99 Allenby St, Tel Aviv-Yafo
    34.7880,   // Shaul Hamelekh, Tel Aviv
    34.7712,   // 99 Hayarkon St, Tel Aviv-Yafo
    35.2137,   // Yafo, Jerusalem
    35.2217,   // 23 King David St, Jerusalem
    35.2281,   // 40 Emek Refaim, Jerusalem
    34.8866,   // 2 Weizman St, Ra'Anana
    34.9032,   // 7 Ezer Weizman St, Hod Hasharon
    34.9896,   // 12 Sderot Ben Gurion, Haifa
    34.9858,   // 24 Herzl St, Haifa
    34.9956,   // 109 Hanassi Blvd, Haifa
    35.0073,   // 29 Yerushalayim St, Haifa
    34.9939,   // 7 Ma'Abarot, Haifa
    34.8670,   // 28 Herzl, Netanya
    34.8116,   // 8 Hadegel St, Tel Aviv-Yafo
    34.8125,   // 36 Ha'Atsmaut Blvd, Bat Yam
    34.6435,   // 11 Ha-Rav Nisanbaum St, Bat Yam
    34.6487,   // Hanamal, Ashdod
    34.6491,   // Sderot Menachem Begin, Ashdod
    34.5717,   // Moshe Dayan Avenue, Ashdod
    34.5732,   // 1 David Ben Gurion Avenue, Ashkelon
    34.9162,   // 1 Arye Tagar St, Ashkelon
    34.9220,   // 40 Sderot Rothschild, Hadera
    34.8944,   // 26 Hasela St, Atlit
    34.8961,   // 21 Herzl St, Lod
    34.8700,   // 2 Dani Mass St, Lod
    34.8390,   // 2 David Remez St, Or Yehuda
    34.8462,   // 9 Hagdud Haivri St, Ra'anana
    34.8408,   // 26 Haahot Haya Oster St 2, Ramat Gan
    34.8522,   // 95 Ahuza St, Ra'anana
    34.8555,   // 5 Keren Hayesod St, Ra'anana
    34.8689,   // 6 Derech Jaffa, Tel Aviv-Yafo
    34.8803,   // 1 Menachem Begin, Petah Tikva
    34.9445,   // 1 Hamalbim, Jerusalem
    34.8324,   // 94 Em Hamoshavot Rd, Petah Tikva
    34.8420,   // 157 Weizmann St, Kfar Saba
    34.8402,   // 13 Ha-Palmakh St, Ra'anana
    34.8425,   // 77 Haenergia St, Be'er Sheva
    35.5318,   // 5 Ha-Palmakh St, Be'er Sheva
    35.5779,   // 66 Harav Kook St, Herzliya
    34.7918,   // 14 Yehuda Halevi St, Herzliya
    34.6810,   // 43 Ha-Galil St, Ra'anana
    35.5318,   // Hayam, Tiberias
    35.5779,   // 106 Tel Hai St, Qiryat Shemona
    34.7918,   // 64 Sderot Haatsma'Ut, Kiryat Gat
    34.6810    // 2 HaDekel, Netivot
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
            double? MaxDistance = 10 + s_rand.NextDouble() * (40 - 10);

            Volunteer newV = new Volunteer(id, FullNamesVolunteers[i], PhonesVolunteers[i], EmailsVolunteers[i],
                PasswordsVolunteers[i], FullAddressesVolunteers[i], LatitudeVolunteers[i], LongitudeVolunteers[i],
                RolesVolunteers[i], true, MaxDistance);

            // Create the volunteer in the DAL.
            s_dal!.Volunteer.Create(newV);

            // Increment the index for the next iteration.
            i++;
        }
        System.Diagnostics.Debug.WriteLine(i);


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
    TYPEOFCALL.CARBURGLARY, TYPEOFCALL.REDRIVE, TYPEOFCALL.FLATTIRE,TYPEOFCALL.REDRIVE, TYPEOFCALL.FLATTIRE,
    TYPEOFCALL.FLATTIRE, TYPEOFCALL.CARBURGLARY, TYPEOFCALL.REDRIVE, TYPEOFCALL.FLATTIRE, TYPEOFCALL.CARBURGLARY,
    TYPEOFCALL.REDRIVE, TYPEOFCALL.FLATTIRE, TYPEOFCALL.REDRIVE, TYPEOFCALL.CARBURGLARY, TYPEOFCALL.FLATTIRE,
    TYPEOFCALL.CARBURGLARY, TYPEOFCALL.REDRIVE, TYPEOFCALL.FLATTIRE,TYPEOFCALL.REDRIVE, TYPEOFCALL.FLATTIRE
        };
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
    "A child in his 30s calls for help after discovering an issue with the car's alignment. He can’t start the car.",
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
    "Jabotinsky St, Ramat Gan",
    "Rothschild Blvd, Tel Aviv",
    "HaMeYasdim St, Moshav Maor",
    "HaMaayan St, Ein Kerem, Jerusalem",
    "Uruguay St, Kiryat Yovel, Jerusalem",
    "Fern St, Ramat Eshkol, Jerusalem",
    "Hillel St, Haifa",
    "Yigal Alon St, Tel Aviv",
    "Neve Tzedek St, Tel Aviv",
    "Herzl St, Be'er Sheva",
    "Shalom Aleichem St, Netanya",
    "Dizengoff St, Tel Aviv",
    "Bialik St, Herzliya",
    "Golomb St, Kiryat Moshe, Jerusalem",
    "Herzl St, Rehovot",
    "HaNasi Blvd, Ashkelon",
    "Yefet St, Jaffa, Tel Aviv",
    "HaAlon St, Eilat",
    "HaTzabar St, Ramat Gan",
    "Shuk HaCarmel, Tel Aviv",
    "Maayan St, Raanana",
    "HaHadarim St, Zichron Yaakov",
    "Derech Akko, Acre",
    "HaShita St, Nesher",
    "Arlozorov St, Ramat Aviv, Tel Aviv",
    "HaNasi Blvd, HaCarmel, Haifa",
    "HaTapuach St, Rehovot",
    "Menachem Begin Blvd, Tel Aviv",
    "Ben Tov St, Ramat Shlomo, Jerusalem",
    "Bar Ilan St, Ramat Gan",
    "Yarkon Park, Tel Aviv",
    "HaBanim St, Binyamina",
    "HaArava St, Eilat",
    "HaDagan St, Moshav Sde Warburg",
    "Kikar Hamedina, Tel Aviv",
    "HaShaked St, Ramat Gan",
    "HaSharon St, Kfar Saba",
    "HaGolan St, Ashdod",
    "Oded St, Petah Tikva",
    "Tchernichovsky St, Tel Aviv",
    "Ibn Gabirol St, Lev Ha'Ir, Tel Aviv",
    "HaZohar St, Herzliya",
    "HaPrachim St, Herzliya Pituach",
    "HaNegev Parking, Be'er Sheva",
    "HaShizaf St, Rehovot",
    "Zanvil St, Petah Tikva",
    "Sderot, Kiryat Gat",
    "Shalom Aleichem St, Rishon Lezion",
    "HaSadot St, Ashkelon",
    "HaTmarim St, Ramat Gan",
    "HaTzayarim St, Kfar Saba",
    "Derech HaYam, Tel Aviv",
    "Histadrut Blvd, Haifa",
    "HaHaroshet St, Petah Tikva",
    "Anilevich St, Kiryat Motzkin",
    "King George St, Jerusalem",
    "Sheshet HaYamim St, Be'er Sheva",
    "HaOn St, Netanya",
    "Emek Dotan St, Raanana",
    "Aharonson St, Tel Aviv",
    "Yaakov Dori St, Rishon Lezion",
    "Even-Shoshan St, Herzliya",
    "HaShisha St, Ashkelon",
    "Nordau St, Nahariya",
    "Yotam St, Modi'in Illit"
};


        double[] LatitudeOfCall = {
    32.0717, // Jabotinsky St, Ramat Gan
    32.0715, // Rothschild Blvd, Tel Aviv
    32.4253, // HaMeYasdim St, Moshav Maor
    31.7681, // HaMaayan St, Ein Kerem, Jerusalem
    31.7618, // Uruguay St, Kiryat Yovel, Jerusalem
    31.8049, // Fern St, Ramat Eshkol, Jerusalem
    32.7937, // Hillel St, Haifa
    32.0722, // Yigal Alon St, Tel Aviv
    32.0628, // Neve Tzedek St, Tel Aviv
    31.2526, // Herzl St, Be'er Sheva
    32.3276, // Shalom Aleichem St, Netanya
    32.0831, // Dizengoff St, Tel Aviv
    32.1663, // Bialik St, Herzliya
    31.7857, // Golomb St, Kiryat Moshe, Jerusalem
    31.8947, // Herzl St, Rehovot
    31.6687, // HaNasi Blvd, Ashkelon
    32.0526, // Yefet St, Jaffa, Tel Aviv
    29.5569, // HaAlon St, Eilat
    32.0729, // HaTzabar St, Ramat Gan
    32.0673, // Shuk HaCarmel, Tel Aviv
    32.1856, // Maayan St, Raanana
    32.5724, // HaHadarim St, Zichron Yaakov
    32.9287, // Derech Akko, Acre
    32.7661, // HaShita St, Nesher
    32.1009, // Arlozorov St, Ramat Aviv, Tel Aviv
    32.8080, // HaNasi Blvd, HaCarmel, Haifa
    31.8988, // HaTapuach St, Rehovot
    32.0631, // Menachem Begin Blvd, Tel Aviv
    31.8100, // Ben Tov St, Ramat Shlomo, Jerusalem
    32.0747, // Bar Ilan St, Ramat Gan
    32.1030, // Yarkon Park, Tel Aviv
    32.5222, // HaBanim St, Binyamina
    29.5606, // HaArava St, Eilat
    32.2036, // HaDagan St, Moshav Sde Warburg
    32.0792, // Kikar Hamedina, Tel Aviv
    32.0624, // HaShaked St, Ramat Gan
    32.1706, // HaSharon St, Kfar Saba
    31.7951, // HaGolan St, Ashdod
    32.0872, // Oded St, Petah Tikva
    32.0716, // Tchernichovsky St, Tel Aviv
    32.0782, // Ibn Gabirol St, Lev Ha'Ir, Tel Aviv
    32.1691, // HaZohar St, Herzliya
    32.1770, // HaPrachim St, Herzliya Pituach
    31.2514, // HaNegev Parking, Be'er Sheva
    31.8841, // HaShizaf St, Rehovot
    32.0768, // Zanvil St, Petah Tikva
    31.6377, // Sderot, Kiryat Gat
    31.9686, // Shalom Aleichem St, Rishon Lezion
    31.6666, // HaSadot St, Ashkelon
    32.0697, // HaTmarim St, Ramat Gan
    32.1729, // HaTzayarim St, Kfar Saba
    32.0732, // Derech HaYam, Tel Aviv
    32.7957, // Histadrut Blvd, Haifa
    32.0919, // HaHaroshet St, Petah Tikva
    32.8333, // Anilevich St, Kiryat Motzkin
    31.7820, // King George St, Jerusalem
    31.2562, // Sheshet HaYamim St, Be'er Sheva
    32.3298, // HaOn St, Netanya
    32.1903, // Emek Dotan St, Raanana
    32.0717, // Aharonson St, Tel Aviv
    31.9649, // Yaakov Dori St, Rishon Lezion
    32.1724, // Even-Shoshan St, Herzliya
    31.6705, // HaShisha St, Ashkelon
    33.0039, // Nordau St, Nahariya
    31.8906  // Yotam St, Modi'in Illit

};

        double[] LongitudeOfCall = {
    34.8021, // Jabotinsky St, Ramat Gan
    34.7744, // Rothschild Blvd, Tel Aviv
    35.0044, // HaMeYasdim St, Moshav Maor
    35.1622, // HaMaayan St, Ein Kerem, Jerusalem
    35.1764, // Uruguay St, Kiryat Yovel, Jerusalem
    35.2155, // Fern St, Ramat Eshkol, Jerusalem
    34.9892, // Hillel St, Haifa
    34.7915, // Yigal Alon St, Tel Aviv
    34.7674, // Neve Tzedek St, Tel Aviv
    34.7997, // Herzl St, Be'er Sheva
    34.8643, // Shalom Aleichem St, Netanya
    34.7766, // Dizengoff St, Tel Aviv
    34.8433, // Bialik St, Herzliya
    35.1979, // Golomb St, Kiryat Moshe, Jerusalem
    34.8102, // Herzl St, Rehovot
    34.5742, // HaNasi Blvd, Ashkelon
    34.7554, // Yefet St, Jaffa, Tel Aviv
    34.9516, // HaAlon St, Eilat
    34.8159, // HaTzabar St, Ramat Gan
    34.7686, // Shuk HaCarmel, Tel Aviv
    34.8860, // Maayan St, Raanana
    34.9546, // HaHadarim St, Zichron Yaakov
    35.0764, // Derech Akko, Acre
    35.0315, // HaShita St, Nesher
    34.8037, // Arlozorov St, Ramat Aviv, Tel Aviv
    34.9896, // HaNasi Blvd, HaCarmel, Haifa
    34.8039, // HaTapuach St, Rehovot
    34.7738, // Menachem Begin Blvd, Tel Aviv
    35.2198, // Ben Tov St, Ramat Shlomo, Jerusalem
    34.8239, // Bar Ilan St, Ramat Gan
    34.8130, // Yarkon Park, Tel Aviv
    34.9450, // HaBanim St, Binyamina
    34.9482, // HaArava St, Eilat
    34.9072, // HaDagan St, Moshav Sde Warburg
    34.7865, // Kikar Hamedina, Tel Aviv
    34.8164, // HaShaked St, Ramat Gan
    34.9126, // HaSharon St, Kfar Saba
    34.6621, // HaGolan St, Ashdod
    34.8732, // Oded St, Petah Tikva
    34.7709, // Tchernichovsky St, Tel Aviv
    34.7828, // Ibn Gabirol St, Lev Ha'Ir, Tel Aviv
    34.8624, // HaZohar St, Herzliya
    34.8041, // HaPrachim St, Herzliya Pituach
    34.7924, // HaNegev Parking, Be'er Sheva
    34.8122, // HaShizaf St, Rehovot
    34.8690, // Zanvil St, Petah Tikva
    34.6146, // Sderot, Kiryat Gat
    34.8055, // Shalom Aleichem St, Rishon Lezion
    34.5801, // HaSadot St, Ashkelon
    34.8232, // HaTmarim St, Ramat Gan
    34.9130, // HaTzayarim St, Kfar Saba
    34.7728, // Derech HaYam, Tel Aviv
    35.0069, // Histadrut Blvd, Haifa
    34.8872, // HaHaroshet St, Petah Tikva
    35.0833, // Anilevich St, Kiryat Motzkin
    35.2173, // King George St, Jerusalem
    34.7915, // Sheshet HaYamim St, Be'er Sheva
    34.8587, // HaOn St, Netanya
    34.8692, // Emek Dotan St, Raanana
    34.7844, // Aharonson St, Tel Aviv
    34.7972, // Yaakov Dori St, Rishon Lezion
    34.8569, // Even-Shoshan St, Herzliya
    34.5772, // HaShisha St, Ashkelon
    35.0805, // Nordau St, Nahariya
    35.0084  // Yotam St, Modi'in Illit


};

        // Array of timestamps representing the creation time of each call
                DateTime[] OpenTimeOfCalls = {
    new DateTime(2025, 06, 25, 14, 32, 10),
    new DateTime(2025, 06, 18, 09, 47, 35),
    new DateTime(2025, 06, 11, 16, 15, 42),
    new DateTime(2025, 06, 04, 11, 08, 23),
    new DateTime(2024, 05, 28, 17, 29, 05),
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
    new DateTime(2025, 07, 16, 09, 38, 47),
    new DateTime(2025, 07, 16 ,11, 10, 59),
    new DateTime(2025, 07, 16, 08, 48, 33),
    new DateTime(2025, 07, 16, 12, 27, 41),
    new DateTime(2025, 07, 16, 17, 45, 18),
    new DateTime(2025, 07, 16, 10, 53, 26),
    new DateTime(2025, 07, 16, 07, 15, 32),
    new DateTime(2025, 07, 16, 13, 44, 09),
    new DateTime(2025, 07, 16, 16, 11, 00),
    new DateTime(2025, 07, 16, 09, 59, 05),
    new DateTime(2025, 07, 15, 11, 35, 47),
    new DateTime(2025, 07, 15, 08, 20, 31),
    new DateTime(2025, 07, 16, 15, 25, 13),
    new DateTime(2025, 07, 14, 14, 44, 22),
    new DateTime(2025, 07, 15, 14, 44, 22),
 new DateTime(2025, 07, 13, 14, 44, 22),
   new DateTime(2025, 07, 16, 14, 44, 22),
   new DateTime(2025, 07, 17, 12, 44, 22),
    new DateTime(2025, 07, 15, 14, 44, 22),
    new DateTime(2025, 07, 15, 14, 44, 22),
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
    new DateTime(2025, 07, 12, 16, 11, 29),
    new DateTime(2025, 07, 16, 13, 33, 08),
    new DateTime(2025, 07, 17, 09, 26, 40),
   new DateTime(2025, 07, 10, 08, 55, 52),
new DateTime(2025, 07, 09, 10, 00, 00),
new DateTime(2025, 07, 08, 09, 30, 00),
new DateTime(2025, 07, 07, 14, 15, 00),
new DateTime(2025, 07, 06, 08, 45, 00),
new DateTime(2025, 07, 05, 11, 20, 00),
new DateTime(2025, 07, 04, 16, 00, 00),
new DateTime(2025, 07, 03, 13, 10, 00),
new DateTime(2025, 07, 02, 17, 50, 00),
new DateTime(2025, 07, 01, 12, 40, 00),
new DateTime(2025, 06, 30, 08, 00, 00),
new DateTime(2025, 06, 29, 10, 10, 00),
new DateTime(2025, 06, 28, 15, 45, 00),
new DateTime(2025, 06, 27, 09, 00, 00),
new DateTime(2025, 06, 26, 11, 30, 00),
new DateTime(2025, 06, 25, 13, 55, 00),

};




        // Array of timestamps representing the expected completion time of each call
        DateTime?[] MaxTimeToFinishOfCalls ={
    new DateTime(2025, 06, 27, 14, 32, 10),
   new DateTime(2025, 06, 22, 00, 51, 54),
     new DateTime(2025, 06, 11, 19, 15, 42),
    new DateTime(2025, 06, 05, 11, 08, 23),
   new DateTime(2024, 05, 29, 17, 29, 05),
  new DateTime(2025, 05, 21, 20, 55, 12),
    new DateTime(2025, 05, 16, 18, 40, 51),
    new DateTime(2025, 05, 08, 15, 18, 44),
new DateTime(2025, 05, 01, 07, 22, 19),
 new DateTime(2025, 04, 24, 19, 33, 37),
    new DateTime(2025, 04, 16, 12, 15, 00),
    new DateTime(2025, 04, 11, 13, 12, 09),
new DateTime(2025, 04, 02, 19, 44, 55),
     new DateTime(2025, 03, 27, 18, 03, 28),
    new DateTime(2025, 03, 19, 19, 25, 13),
     new DateTime(2025, 07, 25, 09, 38, 47),
    new DateTime(2025, 07, 24 ,11, 10, 59),
    new DateTime(2025, 07, 23, 08, 48, 33),
    new DateTime(2025, 07, 23, 12, 27, 41),
    new DateTime(2025, 07, 24, 17, 45, 18),
    new DateTime(2025, 07, 21, 10, 53, 26),
    new DateTime(2025, 07, 21, 07, 15, 32),
    new DateTime(2025, 07, 21, 13, 44, 09),
    new DateTime(2025, 07, 22, 16, 11, 00),
    new DateTime(2025, 07, 22, 09, 59, 05),
    new DateTime(2025, 07, 22, 11, 35, 47),
    new DateTime(2025, 07, 22, 08, 20, 31),
    new DateTime(2025, 07, 29, 15, 25, 13),
    new DateTime(2025, 07, 28, 14, 44, 22),
    new DateTime(2025, 07, 28, 14, 44, 22),
 new DateTime(2025, 07, 28, 14, 44, 22),
   new DateTime(2025, 07, 21, 14, 44, 22),
   new DateTime(2025, 07, 25, 12, 44, 22),
    new DateTime(2025, 07, 25, 14, 44, 22),
    new DateTime(2025, 07, 25, 14, 44, 22),
      new DateTime(2024, 10, 27, 16, 13, 03),
    new DateTime(2024, 10, 20, 09, 42, 27),
    new DateTime(2024, 10, 13, 18, 00, 00),
    new DateTime(2024, 10, 06, 11, 12, 38),
    new DateTime(2024, 09, 29, 08, 29, 21),
    new DateTime(2024, 09, 22, 14, 57, 12),
    new DateTime(2024, 09, 15, 13, 13, 45),
    new DateTime(2024, 09, 08, 10, 36, 53),
     new DateTime(2024, 09, 04, 15, 09, 34),
    new DateTime(2024, 08, 26, 07, 44, 16),
    new DateTime(2024, 08, 16, 12, 00, 00),
    new DateTime(2024, 08, 12, 17, 29, 48),
    new DateTime(2025, 08, 03, 16, 11, 29),
    new DateTime(2025, 08, 23, 13, 33, 08),
    new DateTime(2025, 07, 26, 18, 33, 08),
    new DateTime(2025, 08, 10, 08, 55, 52),
new DateTime(2025, 08, 09, 10, 00, 00),
new DateTime(2025, 08, 08, 09, 30, 00),
new DateTime(2025, 08, 07, 14, 15, 00),
new DateTime(2025, 08, 06, 08, 45, 00),
new DateTime(2025, 08, 05, 11, 20, 00),
new DateTime(2025, 08, 04, 16, 00, 00),
new DateTime(2025, 08, 03, 13, 10, 00),
new DateTime(2025, 08, 02, 17, 50, 00),
new DateTime(2025, 08, 01, 12, 40, 00),
new DateTime(2025, 07, 30, 08, 00, 00),
new DateTime(2025, 07, 29, 10, 10, 00),
new DateTime(2025, 07, 28, 15, 45, 00),
new DateTime(2025, 07, 27, 09, 00, 00),
new DateTime(2025, 07, 26, 11, 30, 00),
new DateTime(2025, 07, 25, 13, 55, 00),


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
        System.Diagnostics.Debug.WriteLine(MaxTimeToFinishOfCalls.Length);  
        System.Diagnostics.Debug.WriteLine(OpenTimeOfCalls.Length);

    }
    /// <summary>
    /// This method reads all the call records and volunteer records from the data access layer.
    /// It uses predefined arrays of treatment statuses, entry times, and end times to create `Assignment` objects.
    /// Each assignment is created by pairing data from the call and volunteer lists at the same index and is then saved to the database.
    /// </summary>
    private static void createAssignment()
    {
        // Array of predefined treatment statuses for each assignment
        TYPEOFTREATMENT?[] treatmentStatuses ={
            TYPEOFTREATMENT.TREATE,
            TYPEOFTREATMENT.TREATE,
            TYPEOFTREATMENT.TREATE,
            TYPEOFTREATMENT.TREATE,
            TYPEOFTREATMENT.TREATE,
            TYPEOFTREATMENT.TREATE,
            TYPEOFTREATMENT.TREATE,
            TYPEOFTREATMENT.TREATE,
            TYPEOFTREATMENT.TREATE,
            TYPEOFTREATMENT.TREATE,
            TYPEOFTREATMENT.TREATE,
            TYPEOFTREATMENT.TREATE,
            TYPEOFTREATMENT.TREATE,
            TYPEOFTREATMENT.TREATE,
            TYPEOFTREATMENT.TREATE,
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
           null,
           null,
           null,
           null,
           null,
           TYPEOFTREATMENT.CANCELLATIONHASEXPIRED,
           TYPEOFTREATMENT.CANCELLATIONHASEXPIRED,
           TYPEOFTREATMENT.CANCELLATIONHASEXPIRED,
           TYPEOFTREATMENT.CANCELLATIONHASEXPIRED,
           TYPEOFTREATMENT.CANCELLATIONHASEXPIRED,
           TYPEOFTREATMENT.CANCELLATIONHASEXPIRED,
           TYPEOFTREATMENT.CANCELLATIONHASEXPIRED,
           TYPEOFTREATMENT.CANCELLATIONHASEXPIRED,
           TYPEOFTREATMENT.CANCELLATIONHASEXPIRED,
          TYPEOFTREATMENT.SELFCANCELLATION,
          TYPEOFTREATMENT.SELFCANCELLATION,
          TYPEOFTREATMENT.SELFCANCELLATION,
          TYPEOFTREATMENT.CANCELINGANADMINISTRATOR,
          TYPEOFTREATMENT.CANCELINGANADMINISTRATOR,
           TYPEOFTREATMENT.CANCELINGANADMINISTRATOR,

        };
        // Array of entry times for treatment assignments
        DateTime[] ArrayOfEntryTimeForTreatment ={
    new DateTime(2025, 06, 26, 00, 51, 54),
    new DateTime(2023, 06, 20, 19, 35, 35),
    new DateTime(2025, 06, 11, 17, 07, 41),
    new DateTime(2025, 06, 04, 14, 08, 23),
    new DateTime(2024, 05, 28, 18, 29, 05),
   new DateTime(2025, 05, 21, 10, 55, 12),
new DateTime(2025, 05, 15, 12, 40, 51),
    new DateTime(2025, 05, 07, 20, 18, 44),
new DateTime(2025, 04, 30, 09, 22, 19),
     new DateTime(2025, 04, 23, 21, 33, 37),
    new DateTime(2025, 04, 16, 07, 15, 00),
    new DateTime(2025, 04, 11, 07, 12, 09),
  new DateTime(2025, 04, 02, 11, 44, 55),
    new DateTime(2025, 03, 26, 20, 03, 28),
   new DateTime(2025, 03, 19, 17, 25, 13),
     new DateTime(2025, 07, 16, 09, 38, 47),
    new DateTime(2025, 07, 16 ,11, 10, 59),
    new DateTime(2025, 07, 16, 08, 48, 33),
    new DateTime(2025, 07, 16, 12, 27, 41),
    new DateTime(2025, 07, 16, 17, 45, 18),
    new DateTime(2025, 07, 16, 10, 53, 26),
    new DateTime(2025, 07, 16, 07, 15, 32),
    new DateTime(2025, 07, 16, 13, 44, 09),
    new DateTime(2025, 07, 16, 16, 11, 00),
    new DateTime(2025, 07, 16, 09, 59, 05),
    new DateTime(2025, 07, 15, 11, 35, 47),
    new DateTime(2025, 07, 15, 08, 20, 31),
    new DateTime(2025, 07, 16, 15, 25, 13),
    new DateTime(2025, 07, 14, 14, 44, 22),
    new DateTime(2025, 07, 15, 14, 44, 22),
 new DateTime(2025, 07, 13, 14, 44, 22),
   new DateTime(2025, 07, 16, 14, 44, 22),
   new DateTime(2025, 07, 17, 12, 44, 22),
    new DateTime(2025, 07, 15, 14, 44, 22),
    new DateTime(2025, 07, 15, 14, 44, 22),
     new DateTime(2024, 10, 24, 16, 13, 03),
    new DateTime(2024, 10, 17, 09, 42, 27),
    new DateTime(2024, 10, 10, 18, 00, 00),
    new DateTime(2024, 10, 03, 11, 12, 38),
    new DateTime(2024, 09, 26, 08, 29, 21),
    new DateTime(2024, 09, 19, 14, 57, 12),
    new DateTime(2024, 09, 12, 13, 13, 45),
    new DateTime(2024, 09, 05, 10, 36, 53),
     new DateTime(2024, 08, 29, 15, 09, 34),
    new DateTime(2024, 08, 22, 07, 44, 16),
    new DateTime(2024, 08, 15, 12, 00, 00),
    new DateTime(2024, 08, 16, 12, 00, 00),
    new DateTime(2024, 08, 08, 17, 29, 48),
    new DateTime(2025, 07, 13, 16, 11, 29),
    new DateTime(2025, 07, 17, 13, 33, 08),
};


        // Array of end times for treatment assignments
        DateTime?[] ArrayOfEndTimeOfTreatment ={
new DateTime(2025, 06, 26, 14, 51, 54),
new DateTime(2024, 06, 21, 00, 51, 54),
new DateTime(2025, 06, 11, 18, 07, 41),
new DateTime(2025, 06, 04, 16, 08, 23),
 new DateTime(2024, 05, 28, 21, 29, 05),
new DateTime(2025, 05, 21, 13, 55, 12),
new DateTime(2025, 05, 15, 15, 40, 51),
   new DateTime(2025, 05, 07, 22, 18, 44),
 new DateTime(2025, 04, 30, 10, 50, 19),
new DateTime(2025, 04, 23, 23, 33, 37),
new DateTime(2025, 04, 16, 10, 15, 00),
new DateTime(2025, 04, 11, 09, 12, 09),
new DateTime(2025, 04, 02, 12, 44, 55),
 new DateTime(2025, 03, 26, 23, 03, 28),
new DateTime(2025, 03, 19, 18, 25, 13),
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
null,
null,
null,
null,
null,
    new DateTime(2024, 10, 28, 16, 13, 03),
    new DateTime(2024, 10, 21, 09, 42, 27),
    new DateTime(2024, 10, 14, 18, 00, 00),
    new DateTime(2024, 10, 07, 11, 12, 38),
    new DateTime(2024, 10, 01, 08, 29, 21),
    new DateTime(2024, 09, 23, 14, 57, 12),
    new DateTime(2024, 09, 17, 13, 13, 45),
    new DateTime(2024, 09, 09, 10, 36, 53),
    new DateTime(2024, 09, 05, 15, 09, 34),
null,
null,
null,
null,
null,
null,


};
        Random rnd = new Random();

        List<Call> allCalls = s_dal!.Call.ReadAll().ToList();
        List<Volunteer> allVolunteers = s_dal!.Volunteer.ReadAll().ToList();

        int totalVolunteers = allVolunteers.Count;

        var inactiveVolunteers = allVolunteers.OrderBy(_ => rnd.Next()).Take((int)(totalVolunteers * 0.1)).ToList();
        var remainingAfterInactive = allVolunteers.Except(inactiveVolunteers).ToList();
        var lightVolunteers = remainingAfterInactive.OrderBy(_ => rnd.Next()).Take((int)(totalVolunteers * 0.3)).ToList();
        var activeVolunteers = remainingAfterInactive.Except(lightVolunteers).ToList();

        List<Assignment> assignments = new List<Assignment>();

        int callIndex = 0;
        System.Diagnostics.Debug.WriteLine(ArrayOfEntryTimeForTreatment.Length);
        System.Diagnostics.Debug.WriteLine(ArrayOfEndTimeOfTreatment.Length);
        System.Diagnostics.Debug.WriteLine(allCalls.Count);
        System.Diagnostics.Debug.WriteLine(allVolunteers.Count);
        System.Diagnostics.Debug.WriteLine(lightVolunteers.Count);

        // מתנדבים Light – הקצאה אחת בלבד
        for (int i = 0; i < lightVolunteers.Count && callIndex < allCalls.Count; i++)
        {
            Assignment newA = new Assignment(0, allCalls[callIndex].Id, lightVolunteers[i].Id,
                ArrayOfEntryTimeForTreatment[callIndex],
                ArrayOfEndTimeOfTreatment[callIndex],
                treatmentStatuses[callIndex]);

            s_dal!.Assignment.Create(newA);
            assignments.Add(newA);
            callIndex++;
        }

        // מתנדבים Active – 2 או 3 קריאות, מקסימום אחת בטיפול
        for (int i = 0; i < activeVolunteers.Count && callIndex < allCalls.Count - 16; i++)
        {
            var volunteerId = activeVolunteers[i].Id;
            int numCalls = rnd.Next(2, 4);
            int assignedCalls = 0;

            bool alreadyHasInTreatment = assignments.Any(a =>
                a.VolunteerId == volunteerId &&
                !a.EndTimeOfTreatment.HasValue &&
                a.TypeOfTreatment == null);

            while (assignedCalls < numCalls && callIndex < allCalls.Count - 16)
            {
                var call = allCalls[callIndex];

                // בדיקה אם הקריאה הנוכחית נחשבת בטיפול
                bool isCallInTreatment =
                    !ArrayOfEndTimeOfTreatment[callIndex].HasValue &&
                    treatmentStatuses[callIndex] == null;

                // אם זו קריאה בטיפול ולמתנדב כבר יש אחת בטיפול – נדלג
                if (isCallInTreatment && alreadyHasInTreatment)
                {
                   
                    break; // ננסה את הקריאה הזו עם מתנדב אחר
                }


                Assignment newA = new Assignment(0, call.Id, volunteerId,
                    ArrayOfEntryTimeForTreatment[callIndex],
                    ArrayOfEndTimeOfTreatment[callIndex],
                    treatmentStatuses[callIndex]);

                s_dal!.Assignment.Create(newA);
                assignments.Add(newA);

                assignedCalls++;
                callIndex++;

                if (isCallInTreatment)
                    alreadyHasInTreatment = true;
            }
        }

        // עדכון מתנדבים שאין להם אף הקצאה – active = false
        for (int j = 0; j < allVolunteers.Count; j++)
        {
            var volunteer = allVolunteers[j];
            if (!assignments.Any(a => a.VolunteerId == volunteer.Id))
            {
                volunteer = volunteer with { Active = false };
                s_dal.Volunteer.Update(volunteer);
            }
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

        //XMLTools.SaveListToXMLSerializer(new List<DO.Volunteer>(), "volunteers.xml");
        //XMLTools.SaveListToXMLSerializer(new List<DO.Call>(), "calls.xml");
        //XMLTools.SaveListToXMLSerializer(new List<DO.Assignment>(), "assignments.xml");
        //XMLTools.SetConfigIntVal("data-config.xml", "NextCallId", 1);
        //XMLTools.SetConfigIntVal("data-config.xml", "NextAssignmentId", 1);


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




