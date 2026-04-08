using SW.ENTITY.Dtos.Dashboard;
using SW.MANAGER.Interfaces;
using SW.REPOSITORY.Interfaces;

namespace SW.MANAGER.Services;

public class DashboardManager : IDashboardManager
{
    private readonly IProductRepository _productRepository;
    private readonly IWarehouseRepository _warehouseRepository;
    private readonly IStorageLocationRepository _storageLocationRepository;
    private readonly IStockTransactionRepository _stockTransactionRepository;

    public DashboardManager(
        IProductRepository productRepository,
        IWarehouseRepository warehouseRepository,
        IStorageLocationRepository storageLocationRepository,
        IStockTransactionRepository stockTransactionRepository)
    {
        _productRepository = productRepository;
        _warehouseRepository = warehouseRepository;
        _storageLocationRepository = storageLocationRepository;
        _stockTransactionRepository = stockTransactionRepository;
    }

    public async Task<DashboardSummaryDto> GetSummaryAsync(string companyId)
    {
        if (string.IsNullOrWhiteSpace(companyId))
        {
            return new DashboardSummaryDto();
        }

        var totalProductCount = await _productRepository.CountAsync(companyId);
        var totalStockCount = await _productRepository.SumCurrentStockAsync(companyId);
        var criticalStockProductCount = await _productRepository.CountCriticalStockAsync(companyId);
        var todayStockInCount = await _stockTransactionRepository.CountTodayInAsync(companyId);
        var todayStockOutCount = await _stockTransactionRepository.CountTodayOutAsync(companyId);
        var totalWarehouseCount = await _warehouseRepository.CountAsync(companyId);
        var totalLocationCount = await _storageLocationRepository.CountAsync(companyId);

        return new DashboardSummaryDto
        {
            TotalProductCount = totalProductCount,
            TotalStockCount = totalStockCount,
            CriticalStockProductCount = criticalStockProductCount,
            TodayStockInCount = todayStockInCount,
            TodayStockOutCount = todayStockOutCount,
            TotalWarehouseCount = totalWarehouseCount,
            TotalLocationCount = totalLocationCount
        };
    }
}