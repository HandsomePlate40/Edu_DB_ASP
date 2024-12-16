using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Edu_DB_ASP.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

public class QuestRepository
{
    private readonly EduDbContext _context;

    public QuestRepository(EduDbContext context)
    {
        _context = context;
    }

    public async Task<List<Participant>> GetQuestMembersAsync(int learnerId)
    {
        var participants = new List<Participant>();

        using (var command = _context.Database.GetDbConnection().CreateCommand())
        {
            command.CommandText = "QuestMembers";
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter("@LearnerID", learnerId));

            _context.Database.OpenConnection();

            using (var result = await command.ExecuteReaderAsync())
            {
                while (await result.ReadAsync())
                {
                    participants.Add(new Participant
                    {
                        QuestId = result.GetInt32(0),
                        QuestTitle = result.GetString(1),
                        LearnerId = result.GetInt32(2),
                        FirstName = result.GetString(3),
                        LastName = result.GetString(4)
                    });
                }
            }
        }

        return participants;
    }
}