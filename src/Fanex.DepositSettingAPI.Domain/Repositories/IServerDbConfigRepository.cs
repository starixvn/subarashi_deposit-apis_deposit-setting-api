namespace Fanex.DepositSettingAPI.Domain.Repositories;

using Entities;

public interface IServerDbConfigRepository
{
    Task<ServerDbConfig> GetServerDbConfigByAgentIdAsync(int agentId);
}