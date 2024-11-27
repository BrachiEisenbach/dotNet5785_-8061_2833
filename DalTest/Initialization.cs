
namespace DalTest;
using DalApi;
using DO;
using System.Data;

public static class Initialization
{
    private static IVolunteer? s_dalVolunteer;
    private static ICall? s_dalCall;
    private static IAssignment? s_dalAssignment;
    private static IConfig? s_dalConfig;
    private static readonly Random s_rand = new();


    private static void createVolunteer()
    {
        //the data was written by AI
        int[] IdsVolunteers = { 12345, 67890, 23456, 78901, 34567, 89012, 45678, 90123, 56789, 12367,
              23489, 34590, 45601, 56702, 67813, 78924, 89035, 90146, 23457, 34568,
              45679, 56780, 67891, 78902, 89013, 90124, 23469, 34570, 45681, 56792,
              67803, 78914, 89025, 90136, 23480, 34591, 45602, 56713, 67824, 78935,
              89046, 90157, 23491, 34502, 45613, 56724, 67835, 78946, 89057, 90168 };
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

    //    foreach (var name in FullNamesVolunteers)
    //    {
    //        int id;
    //        do
    //            id = s_rand.Next(200000000, 400000000);
    //        while (s_dalVolunteer!.Read(id) != null);

    //        string FullName = FullNamesVolunteers[i];
    //        string Phone = PhonesVolunteers[i];
    //        string Email = EmailsVolunteers[i];
    //        string? Password = PasswordsVolunteers[i];
    //        string? FullAddress = FullAddressesVolunteers[i];
    //        ROLE Role = (i==0)?ROLE.ADMIN:RolesVolunteers[i];
    //        bool Active = (id % 2) == 0 ? true : false;
    //        double? MaxDistance = null,
    //TYPEOFDISTSANCE TypeOfDistance = TYPEOFDISTSANCE.AERIALDISTANCE


    //        DateTime start = new DateTime(1995, 1, 1);
    //        DateTime bdt = start.AddDays(s_rand.Next((s_dalConfig.Clock - start).Days));

    //        s_dalVolunteer!.Create(new(id, name, alias, active, bdt));
    //        i++;
    //    }
    }


}
}
