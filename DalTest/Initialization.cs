
namespace DalTest;
using DalApi;
using DO;
using System;
using System.Collections;
using System.Collections.Generic;

public static class Initialization
{
    private static IVolunteer? s_dalVolunteer;
    private static ICall? s_dalCall;
    private static IAssignment? s_dalAssignment;
    private static IConfig? s_dalConfig;
    private static List<Volunteer>? VolunteersArray;
    private static readonly Random s_rand = new();



    private static void createVolunteer()
    {
        //the data was written by AI

        string[] FullNamesVolunteers = { "John Doe", "Jane Smith", "Michael Johnson", "Emily Davis", "Chris Brown",
                       "Jessica Taylor", "David Wilson", "Sophia Moore", "Daniel Anderson", "Olivia Lee",
                       "Matthew Harris", "Lily Clark", "James Walker", "Chloe Hall", "Benjamin Young",
                       "Mia Scott", "Ethan Adams", "Isabella King", "Aiden Martinez", "Charlotte Lee",
                       "Lucas Perez", "Amelia Rodriguez", "Henry Green", "Aria Hall", "Samuel Harris",
                       "Ella Carter", "David Walker", "Avery Thompson", "Oliver Collins", "Sophie White",
                       "William Wright", "Megan Nelson", "Jack Brown", "Madison Roberts", "Jacob Carter",
                       "Sophia Brooks", "Jackson Scott", "Grace Turner", "Mason Miller", "Liam King",
                       "Charlotte Walker", "Daniel Evans", "Ella Green", "Oliver Evans", "Harper Lewis",
                       "Benjamin Lee", "Victoria Allen", "James White", "Natalie Scott", "Evan Phillips",
                       "Zoe Stewart", "Dylan Johnson" };
        string[] PhonesVolunteers = { "052-1234567", "053-2345678", "054-3456789", "055-4567890", "056-5678901",
                    "057-6789012", "058-7890123", "059-8901234", "050-9876543", "051-8765432",
                    "052-7654321", "053-6543210", "054-5432109", "055-4321098", "056-3210987",
                    "057-2109876", "058-1098765", "059-0987654", "050-9876543", "051-8765432",
                    "052-7654321", "053-6543210", "054-5432109", "055-4321098", "056-3210987",
                    "057-2109876", "058-1098765", "059-0987654", "050-9876543", "051-8765432",
                    "052-7654321", "053-6543210", "054-5432109", "055-4321098", "056-3210987",
                    "057-2109876", "058-1098765", "059-0987654", "050-9876543", "051-8765432",
                    "052-7654321", "053-6543210", "054-5432109", "055-4321098", "056-3210987",
                    "057-2109876", "058-1098765", "059-0987654" };
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
                    "natalie.scott@example.com", "evan.phillips@example.com", "zoe.stewart@example.com" };
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
        string[] FullAddressesVolunteers = {
    "Main Street 1, Tel Aviv", "Har 2, Jerusalem", "Independence 45, Haifa", "Galil 12, Petah Tikva",
    "Harbor 8, Ashdod", "Hadar 34, Ramat Gan", "Zamir 56, Netanya", "Yarkon 10, Holon",
    "Hameshe 77, Be'er Sheva", "Shalom 100, Rishon Lezion", "Kokhav 33, Kfar Saba", "Meretz 21, Hadera",
    "Chaim 5, Rehovot", "Egzoz 18, Eilat", "Merkaz 99, Modiin", "Dekel 4, Netivot",
    "Hahatzav 20, Kiryat Gat", "Hahof 3, Bat Yam", "Hamavak 8, Ashkelon", "Hakikar 12, Ramla",
    "Arava 25, Afula", "Hayam 30, Tiberias", "Marpe 40, Raanana", "Technology 67, Petah Tikva",
    "Gefen 22, Be'er Sheva", "Mitzpe 6, Givatayim", "Ogen 75, Azor", "Herpatka 11, Jerusalem",
    "Sela 58, Tirat Carmel", "Atid 3, Kiryat Shmona", "Kokhavim 25, Haifa", "Hama'arav 9, Acre",
    "Penina 14, Bnei Brak", "Ir 90, Lod", "Gan 11, Kfar Yona", "Flag 77, Netanya",
    "Boker 45, Eilat", "Masila 32, Modiin", "Yarden 3, Nahariya", "Ayin 50, Migdal HaEmek",
    "Dekel 99, Petah Tikva", "Kfar 8, Gedera", "Nof 3, Herzliya", "Geva 60, Ashkelon",
    "Hok 11, Haifa", "Geshem 15, Safed", "Horef 2, Jerusalem", "Heder 30, Caesarea",
    "Binyan 7, Ashdod", "Derech 56, Tel Aviv", "Geshem 22, Rehovot", "Yaar 13, Jerusalem",
    "Ner 23, Yokneam", "Kibush 5, Petah Tikva", "Mavak 19, Tel Aviv", "Hazon 9, Kiryat Ono" };
        double[] LatitudeVolunteers = [
    -45.27, 32.41, -12.55, 57.80, 29.82, -20.94, 48.16, -38.69, 53.03, 24.91,
    -64.39, 71.12, -3.67, 33.56, 12.73, -51.30, -5.64, 40.58, 11.09, -72.96,
    34.88, -28.14, 61.29, 43.67, -44.51, 37.98, 27.16, -67.89, -1.34, 49.02,
    50.87, -32.54, 65.72, 21.55, -16.90, 54.03, -55.43, 60.44, 19.92, -9.35,
    68.78, -14.47, 36.09, 56.47, -3.25, 45.62, -21.88, 11.68, 53.94, 23.77
];
        double[] LongitudeVolunteers = [
    13.45, -118.65, 23.53, 89.45, -62.34, 31.22, -15.78, 78.64, -106.83, 45.99,
    -134.56, 72.88, -50.32, 12.96, -82.42, 10.15, 47.72, -68.09, 134.54, -23.11,
    39.87, -116.97, 107.33, -3.77, 63.62, 80.11, -131.07, -42.55, 141.08, 24.39,
    51.61, -73.90, 137.32, -114.98, -120.60, 48.01, -59.37, 16.94, -80.23, 114.79,
    -160.50, 70.77, 53.20, -9.88, 34.06, -126.77, 96.23, 25.76, -110.89, 6.83
];
        ROLE[] RolesVolunteers = { ROLE.DISTRICTMANAGER, ROLE.VOLUNTEER, ROLE.VOLUNTEER, ROLE.DISTRICTMANAGER,
    ROLE.VOLUNTEER, ROLE.VOLUNTEER, ROLE.VOLUNTEER, ROLE.VOLUNTEER, ROLE.DISTRICTMANAGER,
    ROLE.VOLUNTEER, ROLE.VOLUNTEER, ROLE.VOLUNTEER, ROLE.DISTRICTMANAGER, ROLE.VOLUNTEER,
    ROLE.VOLUNTEER, ROLE.DISTRICTMANAGER, ROLE.VOLUNTEER, ROLE.VOLUNTEER, ROLE.VOLUNTEER,
    ROLE.VOLUNTEER, ROLE.DISTRICTMANAGER, ROLE.VOLUNTEER, ROLE.VOLUNTEER, ROLE.VOLUNTEER,
    ROLE.VOLUNTEER, ROLE.VOLUNTEER, ROLE.VOLUNTEER, ROLE.DISTRICTMANAGER, ROLE.VOLUNTEER,
    ROLE.VOLUNTEER, ROLE.VOLUNTEER, ROLE.DISTRICTMANAGER, ROLE.VOLUNTEER, ROLE.VOLUNTEER,
    ROLE.VOLUNTEER, ROLE.VOLUNTEER, ROLE.VOLUNTEER, ROLE.VOLUNTEER, ROLE.DISTRICTMANAGER,
    ROLE.VOLUNTEER, ROLE.DISTRICTMANAGER, ROLE.VOLUNTEER, ROLE.VOLUNTEER, ROLE.VOLUNTEER};
        int i = 0;

        foreach (var name in FullNamesVolunteers)
        {
            int id;
            do
                id = s_rand.Next(200000000, 400000000);
            while (s_dalVolunteer!.Read(id) != null);



            string? Password = PasswordsVolunteers[i];
            string? FullAddress = FullAddressesVolunteers[i];
            ROLE Role = (i == 0) ? ROLE.ADMIN : RolesVolunteers[i];
            bool Active = (id % 2) == 0 ? true : false;
            double? MaxDistance = s_rand.Next(0, 12500);


            Volunteer newV = new Volunteer(id, FullNamesVolunteers[i], PhonesVolunteers[i], EmailsVolunteers[i],
                PasswordsVolunteers[i], FullAddressesVolunteers[i], LatitudeVolunteers[i], LongitudeVolunteers[i],
                RolesVolunteers[i], Active, MaxDistance);
            s_dalVolunteer!.Create(newV);

            i++;

        }

    }
    private static void createCall()
    {
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
    TYPEOFCALL.CARBURGLARY, TYPEOFCALL.REDRIVE, TYPEOFCALL.FLATTIRE};
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
    "A man in his 30s calls for help after discovering an issue with the car's alignment. He can’t start the car."
};
        string[] FullAddressesOfCalls = {
    "Rehov Ben Yehuda 15, Tel Aviv", "Rothschild Blvd 29, Tel Aviv", "Moshav Maor 14, Central District", "Ein Kerem 21, Jerusalem",
    "Kiryat Yovel 35, Jerusalem", "Ramat Eshkol 10, Jerusalem", "Hadar 12, Haifa", "Yigal Alon 56, Tel Aviv",
    "Neve Tzedek 7, Tel Aviv", "Hertzel 9, Be'er Sheva", "Shalva 10, Netanya", "Dizengoff 70, Tel Aviv",
    "Bialik 18, Herzliya", "Kiryat Moshe 2, Jerusalem", "Rehovot 4, Rehovot", "Sderot 25, Ashkelon",
    "Jaffa 23, Tel Aviv", "Alon 48, Eilat", "Tzabar 11, Ramat Gan", "Shuk HaCarmel 3, Tel Aviv",
    "Maayan 25, Raanana", "Zichron Yaakov 12, Haifa", "Tzafon 55, Acre", "Nesher 34, Haifa",
    "Ramat Aviv 15, Tel Aviv", "Carmel 9, Haifa", "Eshkol 5, Rehovot", "Menahem Begin Blvd 100, Tel Aviv",
    "Ramat Shlomo 6, Jerusalem", "Bar Ilan 21, Ramat Gan", "Park Hayarkon 20, Tel Aviv", "Binyamina 4, Haifa",
    "Hamarah 18, Eilat", "Moshav Sde Warburg 22, Center District", "Kikar Hamedina 12, Tel Aviv", "Shaked 8, Ramat Gan",
    "Sharon 30, Kfar Saba", "Hagolan 12, Ashdod", "Oded 45, Petah Tikva", "Tchernichovsky 30, Tel Aviv",
    "Lev Ha'ir 19, Tel Aviv", "Zohar 14, Herzliya", "Herzliya Pituach 6, Herzliya", "Negev 24, Be'er Sheva",
    "Rehovot 11, Rehovot", "Zanvil 3, Petah Tikva", "Jabotinsky 45, Ramat Gan", "Sderot 12, Kiryat Gat",
    "Shalom Aleichem 4, Rishon Lezion", "Sderot 30, Ashkelon", "Rishonim 2, Haifa", "Netivot 19, Be'er Sheva",
    "Tzukim 7, Arad", "Giv'atayim 8, Tel Aviv", "Tamar 20, Dead Sea", "Acre 10, Northern District"
};
        double[] LatitudeOfCall = [
   -45.0, 32.0, -12.0, 58.0, 30.0, -21.0, 48.0, -39.0, 53.0, 25.0,
    -64.0, 71.0, -4.0, 34.0, 13.0, -51.0, -6.0, 41.0, 11.0, -73.0,
    35.0, -28.0, 61.0, 44.0, -45.0, 38.0, 27.0, -68.0, -1.0, 49.0,
    51.0, -33.0, 66.0, 22.0, -17.0, 54.0, -55.0, 60.0, 20.0, -9.0,
    69.0, -14.0, 36.0, 56.0, -3.0, 46.0, -22.0, 12.0, 54.0, 24.0];
        double[] LongitudeOfCall = [
    13.0, -119.0, 24.0, 89.0, -62.0, 31.0, -16.0, 79.0, -107.0, 46.0,
    -135.0, 73.0, -50.0, 13.0, -82.0, 10.0, 48.0, -68.0, 135.0, -23.0,
    40.0, -117.0, 107.0, -4.0, 64.0, 80.0, -131.0, -43.0, 141.0, 24.0,
    52.0, -74.0, 137.0, -115.0, -121.0, 48.0, -59.0, 17.0, -80.0, 115.0,
    -161.0, 71.0, 53.0, -10.0, 34.0, -127.0, 96.0, 26.0, -111.0, 7.0];
        DateTime[] OpenTimeOfCalls =
{
    new DateTime(2023, 12, 31, 23, 45, 12),
    new DateTime(2023, 11, 22, 14, 35, 10),
    new DateTime(2023, 10, 15, 10, 25, 45),
    new DateTime(2023, 12, 05, 08, 18, 30),
    new DateTime(2023, 09, 18, 19, 55, 50),
    new DateTime(2023, 12, 03, 13, 30, 05),
    new DateTime(2023, 08, 10, 07, 05, 40),
    new DateTime(2024, 01, 23, 16, 42, 33),
    new DateTime(2023, 11, 09, 09, 50, 25),
    new DateTime(2023, 07, 22, 20, 13, 18),
    new DateTime(2024, 02, 14, 06, 11, 03),
    new DateTime(2023, 09, 29, 18, 22, 48),
    new DateTime(2024, 03, 05, 23, 10, 32),
    new DateTime(2023, 11, 11, 02, 35, 22),
    new DateTime(2023, 12, 16, 14, 50, 05),
    new DateTime(2024, 01, 02, 11, 17, 03),
    new DateTime(2023, 10, 01, 21, 05, 59),
    new DateTime(2023, 12, 24, 10, 30, 35),
    new DateTime(2024, 01, 17, 05, 52, 47),
    new DateTime(2023, 07, 19, 13, 14, 25),
    new DateTime(2023, 09, 27, 08, 08, 55),
    new DateTime(2024, 02, 11, 15, 50, 42),
    new DateTime(2023, 11, 14, 12, 02, 20),
    new DateTime(2023, 08, 25, 22, 16, 04),
    new DateTime(2024, 03, 20, 00, 59, 50),
    new DateTime(2023, 10, 12, 04, 39, 12),
    new DateTime(2023, 09, 17, 19, 22, 01),
    new DateTime(2023, 12, 21, 17, 12, 44),
    new DateTime(2023, 07, 15, 11, 44, 22),
    new DateTime(2024, 01, 30, 03, 30, 59),
    new DateTime(2023, 11, 05, 16, 40, 35),
    new DateTime(2023, 10, 18, 12, 22, 19),
    new DateTime(2023, 09, 23, 14, 35, 05),
    new DateTime(2024, 01, 13, 09, 55, 37),
    new DateTime(2023, 08, 18, 18, 48, 21),
    new DateTime(2024, 02, 01, 23, 15, 50),
    new DateTime(2023, 11, 30, 06, 29, 33),
    new DateTime(2023, 07, 08, 20, 10, 15),
    new DateTime(2023, 09, 12, 04, 25, 40),
    new DateTime(2024, 03, 11, 02, 55, 10),
    new DateTime(2023, 10, 20, 22, 13, 42),
    new DateTime(2023, 08, 12, 07, 05, 02),
    new DateTime(2023, 12, 13, 15, 40, 55),
    new DateTime(2023, 11, 17, 03, 17, 49),
    new DateTime(2024, 01, 25, 09, 13, 27),
    new DateTime(2023, 09, 30, 10, 32, 15),
    new DateTime(2023, 08, 30, 21, 49, 18),
    new DateTime(2024, 03, 07, 17, 56, 28),
    new DateTime(2023, 07, 05, 12, 12, 12),
    new DateTime(2023, 12, 08, 17, 04, 23),
    new DateTime(2024, 01, 04, 22, 11, 45)
};
        DateTime[] MaxTimeToFinishOfCalls ={
    new DateTime(2023, 12, 31, 04, 45, 12),  // +5 hours
    new DateTime(2023, 11, 22, 19, 35, 10),  // +5 hours
    new DateTime(2023, 10, 15, 15, 25, 45),  // +5 hours
    new DateTime(2023, 12, 05, 13, 18, 30),  // +5 hours
    new DateTime(2023, 09, 18, 00, 55, 50),  // +5 hours
    new DateTime(2023, 12, 03, 18, 30, 05),  // +5 hours
    new DateTime(2023, 08, 10, 12, 05, 40),  // +5 hours
    new DateTime(2024, 01, 23, 21, 42, 33),  // +5 hours
    new DateTime(2023, 11, 09, 14, 50, 25),  // +5 hours
    new DateTime(2023, 07, 22, 01, 13, 18),  // +5 hours
    new DateTime(2024, 02, 14, 11, 11, 03),  // +5 hours
    new DateTime(2023, 09, 29, 23, 22, 48),  // +5 hours
    new DateTime(2024, 03, 05, 04, 10, 32),  // +5 hours
    new DateTime(2023, 11, 11, 07, 35, 22),  // +5 hours
    new DateTime(2023, 12, 16, 19, 50, 05),  // +5 hours
    new DateTime(2024, 01, 02, 16, 17, 03),  // +5 hours
    new DateTime(2023, 10, 01, 02, 05, 59),  // +5 hours
    new DateTime(2023, 12, 24, 15, 30, 35),  // +5 hours
    new DateTime(2024, 01, 17, 10, 52, 47),  // +5 hours
    new DateTime(2023, 07, 19, 18, 14, 25),  // +5 hours
    new DateTime(2023, 09, 27, 13, 08, 55),  // +5 hours
    new DateTime(2024, 02, 11, 20, 50, 42),  // +5 hours
    new DateTime(2023, 11, 14, 17, 02, 20),  // +5 hours
    new DateTime(2023, 08, 25, 03, 16, 04),  // +5 hours
    new DateTime(2024, 03, 20, 05, 59, 50),  // +5 hours
    new DateTime(2023, 10, 12, 09, 39, 12),  // +5 hours
    new DateTime(2023, 09, 17, 00, 22, 01),  // +5 hours
    new DateTime(2023, 12, 21, 22, 12, 44),  // +5 hours
    new DateTime(2023, 07, 15, 16, 44, 22),  // +5 hours
    new DateTime(2024, 01, 30, 08, 30, 59),  // +5 hours
    new DateTime(2023, 11, 05, 21, 40, 35),  // +5 hours
    new DateTime(2023, 10, 18, 17, 22, 19),  // +5 hours
    new DateTime(2023, 09, 23, 19, 35, 05),  // +5 hours
    new DateTime(2024, 01, 13, 14, 55, 37),  // +5 hours
    new DateTime(2023, 08, 18, 23, 48, 21),  // +5 hours
    new DateTime(2024, 02, 01, 04, 15, 50),  // +5 hours
    new DateTime(2023, 11, 30, 11, 29, 33),  // +5 hours
    new DateTime(2023, 07, 08, 01, 10, 15),  // +5 hours
    new DateTime(2023, 09, 12, 09, 25, 40),  // +5 hours
    new DateTime(2024, 03, 11, 07, 55, 10),  // +5 hours
    new DateTime(2023, 10, 20, 03, 13, 42),  // +5 hours
    new DateTime(2023, 08, 12, 12, 05, 02),  // +5 hours
    new DateTime(2023, 12, 13, 20, 40, 55),  // +5 hours
    new DateTime(2023, 11, 17, 08, 17, 49),  // +5 hours
    new DateTime(2024, 01, 25, 14, 13, 27),  // +5 hours
    new DateTime(2023, 09, 30, 15, 32, 15),  // +5 hours
    new DateTime(2023, 08, 30, 02, 49, 18),  // +5 hours
    new DateTime(2024, 03, 07, 22, 56, 28),  // +5 hours
    new DateTime(2023, 07, 05, 17, 12, 12),  // +5 hours
    new DateTime(2023, 12, 08, 22, 04, 23),  // +5 hours
    new DateTime(2024, 01, 04, 03, 11, 45)   // +5 hours
};

        int i = 0;

        foreach (var Description in callDescriptions)
        {

            Call newC = new Call(0, TypesOfCalls[i], callDescriptions[i], FullAddressesOfCalls[i], LatitudeOfCall[i],
                LongitudeOfCall[i], OpenTimeOfCalls[i], MaxTimeToFinishOfCalls[i]);

            s_dalCall!.Create(newC);
            i++;

        }


    }
    private static void createAssignment()
    {
        List<Call> CallIds = s_dalCall.ReadAll();
        List<Volunteer> VolunteerIds = s_dalVolunteer.ReadAll();

        TYPEOFTREATMENT[] treatmentStatuses ={
    TYPEOFTREATMENT.SELFCANCELLATION,
    TYPEOFTREATMENT.CANCALINGANADMINISTRATOR,
    TYPEOFTREATMENT.CANCELLATIONHASEXPIRED,
    TYPEOFTREATMENT.SELFCANCELLATION,
    TYPEOFTREATMENT.CANCALINGANADMINISTRATOR,
    TYPEOFTREATMENT.CANCELLATIONHASEXPIRED,
    TYPEOFTREATMENT.SELFCANCELLATION,
    TYPEOFTREATMENT.CANCALINGANADMINISTRATOR,
    TYPEOFTREATMENT.CANCELLATIONHASEXPIRED,
    TYPEOFTREATMENT.SELFCANCELLATION,
    TYPEOFTREATMENT.CANCALINGANADMINISTRATOR,
    TYPEOFTREATMENT.CANCELLATIONHASEXPIRED,
    TYPEOFTREATMENT.SELFCANCELLATION,
    TYPEOFTREATMENT.CANCALINGANADMINISTRATOR,
    TYPEOFTREATMENT.CANCELLATIONHASEXPIRED,
    TYPEOFTREATMENT.SELFCANCELLATION,
    TYPEOFTREATMENT.CANCALINGANADMINISTRATOR,
    TYPEOFTREATMENT.CANCELLATIONHASEXPIRED,
    TYPEOFTREATMENT.SELFCANCELLATION,
    TYPEOFTREATMENT.CANCALINGANADMINISTRATOR,
    TYPEOFTREATMENT.CANCELLATIONHASEXPIRED,
    TYPEOFTREATMENT.SELFCANCELLATION,
    TYPEOFTREATMENT.CANCALINGANADMINISTRATOR,
    TYPEOFTREATMENT.CANCELLATIONHASEXPIRED,
    TYPEOFTREATMENT.SELFCANCELLATION,
    TYPEOFTREATMENT.CANCALINGANADMINISTRATOR,
    TYPEOFTREATMENT.CANCELLATIONHASEXPIRED,
    TYPEOFTREATMENT.SELFCANCELLATION,
    TYPEOFTREATMENT.CANCALINGANADMINISTRATOR,
    TYPEOFTREATMENT.CANCELLATIONHASEXPIRED,
    TYPEOFTREATMENT.SELFCANCELLATION,
    TYPEOFTREATMENT.CANCALINGANADMINISTRATOR,
    TYPEOFTREATMENT.CANCELLATIONHASEXPIRED,
    TYPEOFTREATMENT.SELFCANCELLATION,
    TYPEOFTREATMENT.CANCALINGANADMINISTRATOR,
    TYPEOFTREATMENT.CANCELLATIONHASEXPIRED,
    TYPEOFTREATMENT.SELFCANCELLATION,
    TYPEOFTREATMENT.CANCALINGANADMINISTRATOR,
    TYPEOFTREATMENT.CANCELLATIONHASEXPIRED,
    TYPEOFTREATMENT.SELFCANCELLATION,
    TYPEOFTREATMENT.CANCALINGANADMINISTRATOR,
    TYPEOFTREATMENT.CANCELLATIONHASEXPIRED,
    TYPEOFTREATMENT.SELFCANCELLATION,
    TYPEOFTREATMENT.CANCALINGANADMINISTRATOR,
    TYPEOFTREATMENT.CANCELLATIONHASEXPIRED,
    TYPEOFTREATMENT.SELFCANCELLATION,
    TYPEOFTREATMENT.CANCALINGANADMINISTRATOR,
    TYPEOFTREATMENT.CANCELLATIONHASEXPIRED,
    TYPEOFTREATMENT.SELFCANCELLATION,
    TYPEOFTREATMENT.CANCALINGANADMINISTRATOR,
    TYPEOFTREATMENT.CANCELLATIONHASEXPIRED};
        DateTime[] ArrayOfEntryTimeForTreatment ={
    new DateTime(2023, 12, 31, 23, 55, 12),
    new DateTime(2023, 11, 22, 15, 05, 10),
    new DateTime(2023, 10, 15, 10, 55, 45),
    new DateTime(2023, 12, 05, 08, 50, 30),
    new DateTime(2023, 09, 18, 20, 05, 50),
    new DateTime(2023, 12, 03, 14, 00, 05),
    new DateTime(2023, 08, 10, 08, 45, 40),
    new DateTime(2024, 01, 23, 17, 20, 33),
    new DateTime(2023, 11, 09, 10, 35, 25),
    new DateTime(2023, 07, 22, 20, 43, 18),
    new DateTime(2024, 02, 14, 07, 15, 03),
    new DateTime(2023, 09, 29, 18, 45, 48),
    new DateTime(2024, 03, 05, 23, 40, 32),
    new DateTime(2023, 11, 11, 03, 15, 22),
    new DateTime(2023, 12, 16, 14, 58, 05),
    new DateTime(2024, 01, 02, 11, 40, 03),
    new DateTime(2023, 10, 01, 21, 15, 59),
    new DateTime(2023, 12, 24, 11, 10, 35),
    new DateTime(2024, 01, 17, 06, 22, 47),
    new DateTime(2023, 07, 19, 14, 05, 25),
    new DateTime(2023, 09, 27, 08, 22, 55),
    new DateTime(2024, 02, 11, 15, 35, 42),
    new DateTime(2023, 11, 14, 12, 32, 20),
    new DateTime(2023, 08, 25, 22, 58, 04),
    new DateTime(2024, 03, 20, 01, 29, 50),
    new DateTime(2023, 10, 12, 04, 52, 12),
    new DateTime(2023, 09, 17, 19, 47, 01),
    new DateTime(2023, 12, 21, 17, 47, 44),
    new DateTime(2023, 07, 15, 12, 04, 22),
    new DateTime(2024, 01, 30, 03, 35, 59),
    new DateTime(2023, 11, 05, 17, 55, 35),
    new DateTime(2023, 10, 18, 12, 38, 19),
    new DateTime(2023, 09, 23, 15, 05, 05),
    new DateTime(2024, 01, 13, 09, 45, 37),
    new DateTime(2023, 08, 18, 19, 10, 21),
    new DateTime(2024, 02, 01, 23, 40, 50),
    new DateTime(2023, 11, 30, 06, 56, 33),
    new DateTime(2023, 07, 08, 21, 05, 15),
    new DateTime(2023, 09, 12, 05, 13, 40),
    new DateTime(2024, 03, 11, 03, 13, 10),
    new DateTime(2023, 10, 20, 22, 50, 42),
    new DateTime(2023, 08, 12, 07, 40, 02),
    new DateTime(2023, 12, 13, 16, 25, 55),
    new DateTime(2023, 11, 17, 04, 45, 49),
    new DateTime(2024, 01, 25, 09, 57, 27),
    new DateTime(2023, 09, 30, 11, 03, 15),
    new DateTime(2023, 08, 30, 21, 25, 18),
    new DateTime(2024, 03, 07, 18, 15, 28),
    new DateTime(2023, 07, 05, 12, 47, 12),
    new DateTime(2023, 12, 08, 17, 50, 23),
    new DateTime(2024, 01, 04, 22, 35, 45)
};
        DateTime[] ArrayOfEndTimeOfTreatment ={
    new DateTime(2024, 01, 01, 01, 55, 12),
    new DateTime(2023, 11, 22, 20, 25, 10),
    new DateTime(2023, 10, 15, 16, 25, 45),
    new DateTime(2023, 12, 05, 14, 35, 30),
    new DateTime(2023, 09, 18, 01, 35, 50),
    new DateTime(2023, 12, 03, 19, 30, 05),
    new DateTime(2023, 08, 10, 13, 25, 40),
    new DateTime(2024, 01, 23, 22, 10, 33),
    new DateTime(2023, 11, 09, 15, 10, 25),
    new DateTime(2023, 07, 22, 02, 00, 18),
    new DateTime(2024, 02, 14, 11, 45, 03),
    new DateTime(2023, 09, 29, 23, 52, 48),
    new DateTime(2024, 03, 05, 04, 45, 32),
    new DateTime(2023, 11, 11, 07, 55, 22),
    new DateTime(2023, 12, 16, 20, 30, 05),
    new DateTime(2024, 01, 02, 17, 10, 03),
    new DateTime(2023, 10, 01, 03, 55, 59),
    new DateTime(2023, 12, 24, 15, 55, 35),
    new DateTime(2024, 01, 17, 10, 05, 47),
    new DateTime(2023, 07, 19, 19, 30, 25),
    new DateTime(2023, 09, 27, 13, 40, 55),
    new DateTime(2024, 02, 11, 21, 15, 42),
    new DateTime(2023, 11, 14, 17, 42, 20),
    new DateTime(2023, 08, 25, 03, 46, 04),
    new DateTime(2024, 03, 20, 06, 50, 50),
    new DateTime(2023, 10, 12, 09, 58, 12),
    new DateTime(2023, 09, 17, 00, 57, 01),
    new DateTime(2023, 12, 21, 22, 35, 44),
    new DateTime(2023, 07, 15, 16, 48, 22),
    new DateTime(2024, 01, 30, 08, 55, 59),
    new DateTime(2023, 11, 05, 22, 30, 35),
    new DateTime(2023, 10, 18, 17, 40, 19),
    new DateTime(2023, 09, 23, 19, 25, 05),
    new DateTime(2024, 01, 13, 15, 05, 37),
    new DateTime(2023, 08, 18, 23, 55, 21),
    new DateTime(2024, 02, 01, 04, 30, 50),
    new DateTime(2023, 11, 30, 12, 15, 33),
    new DateTime(2023, 07, 08, 01, 38, 15),
    new DateTime(2023, 09, 12, 09, 52, 40),
    new DateTime(2024, 03, 11, 07, 35, 10),
    new DateTime(2023, 10, 20, 04, 05, 42),
    new DateTime(2023, 08, 12, 12, 30, 02),
    new DateTime(2023, 12, 13, 16, 55, 55),
    new DateTime(2023, 11, 17, 08, 30, 49),
    new DateTime(2024, 01, 25, 14, 20, 27),
    new DateTime(2023, 09, 30, 15, 45, 15),
    new DateTime(2023, 08, 30, 02, 55, 18),
    new DateTime(2024, 03, 07, 23, 00, 28),
    new DateTime(2023, 07, 05, 17, 32, 12),
    new DateTime(2023, 12, 08, 22, 50, 23),
    new DateTime(2024, 01, 04, 03, 45, 45)};



        int i = 0;

        foreach (var call in CallIds)
        {

            Assignment newA = new Assignment(0, CallIds[i].Id, VolunteerIds[i].Id,
                ArrayOfEntryTimeForTreatment[i], ArrayOfEndTimeOfTreatment[i],
                treatmentStatuses[i]);


            s_dalAssignment!.Create(newA);
            i++;
        }
    }

    public static void Do(IVolunteer? dalVolunteer, ICall? dalCall, IAssignment? dalAssignment, IConfig? dalConfig)
    {
        s_dalVolunteer = dalVolunteer ?? throw new NullReferenceException("DAL object can not be null!");
        s_dalCall = dalCall ?? throw new NullReferenceException("DAL object can not be null!");
        s_dalAssignment = dalAssignment ?? throw new NullReferenceException("DAL object can not be null!");
        s_dalConfig = dalConfig ?? throw new NullReferenceException("DAL object can not be null!");
        Console.WriteLine("Reset Configuration values and List values...");

        s_dalConfig.Reset();
        s_dalVolunteer.DeleteAll();
        s_dalCall.DeleteAll();
        s_dalAssignment.DeleteAll();



        Console.WriteLine("Initializing Students list ...");
        createVolunteer();
        createCall();
        createAssignment();


    }


  


  
    




}




