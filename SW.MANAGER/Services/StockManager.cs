using SW.ENTITY.Dtos.Common;
using SW.ENTITY.Dtos.Stock;
using SW.ENTITY.Entities;
using SW.ENTITY.Enums;
using SW.MANAGER.Interfaces;
using SW.REPOSITORY.Interfaces;

namespace SW.MANAGER.Services;

public class StockManager : IStockManager
{
    private readonly IProductRepository _productRepository;
    private readonly IWarehouseRepository _warehouseRepository;
    private readonly IStorageLocationRepository _storageLocationRepository;
    private readonly IStockTransactionRepository _stockTransactionRepository;

    public StockManager(
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

    public async Task<BaseResponseDto> StockInAsync(StockInDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.CompanyId))
        {
            return new BaseResponseDto
            {
                IsSuccess = false,
                Message = "CompanyId alanı zorunludur.",
                StatusCode = 400
            };
        }

        if (dto.ProductId <= 0)
        {
            return new BaseResponseDto
            {
                IsSuccess = false,
                Message = "Geçerli bir ürün seçilmelidir.",
                StatusCode = 400
            };
        }

        if (dto.WarehouseId <= 0)
        {
            return new BaseResponseDto
            {
                IsSuccess = false,
                Message = "Geçerli bir depo seçilmelidir.",
                StatusCode = 400
            };
        }

        if (dto.StorageLocationId <= 0)
        {
            return new BaseResponseDto
            {
                IsSuccess = false,
                Message = "Geçerli bir lokasyon seçilmelidir.",
                StatusCode = 400
            };
        }

        if (dto.Quantity <= 0)
        {
            return new BaseResponseDto
            {
                IsSuccess = false,
                Message = "Miktar sıfırdan büyük olmalıdır.",
                StatusCode = 400
            };
        }

        var product = await _productRepository.GetByIdAsync(dto.ProductId);

        if (product == null)
        {
            return new BaseResponseDto
            {
                IsSuccess = false,
                Message = "Ürün bulunamadı.",
                StatusCode = 404
            };
        }

        if (product.CompanyId != dto.CompanyId)
        {
            return new BaseResponseDto
            {
                IsSuccess = false,
                Message = "Bu ürün için işlem yapma yetkiniz bulunmamaktadır.",
                StatusCode = 403
            };
        }

        var warehouse = await _warehouseRepository.GetByIdAsync(dto.WarehouseId, dto.CompanyId);

        if (warehouse == null)
        {
            return new BaseResponseDto
            {
                IsSuccess = false,
                Message = "Depo bulunamadı.",
                StatusCode = 404
            };
        }

        var location = await _storageLocationRepository.GetByIdAsync(dto.StorageLocationId, dto.WarehouseId, dto.CompanyId);

        if (location == null)
        {
            return new BaseResponseDto
            {
                IsSuccess = false,
                Message = "Lokasyon bulunamadı veya seçilen depoya ait değil.",
                StatusCode = 404
            };
        }

        product.CurrentStock += dto.Quantity;
        product.UpdatedAt = DateTime.UtcNow;

        var transaction = new StockTransaction
        {
            CompanyId = dto.CompanyId.Trim(),
            ProductId = dto.ProductId,
            WarehouseId = dto.WarehouseId,
            StorageLocationId = dto.StorageLocationId,
            TransactionType = StockTransactionType.In,
            Quantity = dto.Quantity,
            TransactionDate = DateTime.UtcNow,
            Description = string.IsNullOrWhiteSpace(dto.Description) ? "Stok girişi yapıldı." : dto.Description.Trim(),
            CreatedAt = DateTime.UtcNow,
            IsDeleted = false
        };

        await _productRepository.UpdateAsync(product);
        await _stockTransactionRepository.AddAsync(transaction);

        await _productRepository.SaveChangesAsync();

        return new BaseResponseDto
        {
            IsSuccess = true,
            Message = "Stok girişi başarıyla yapıldı.",
            StatusCode = 200
        };
    }

    public async Task<BaseResponseDto> StockOutAsync(StockOutDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.CompanyId))
        {
            return new BaseResponseDto
            {
                IsSuccess = false,
                Message = "CompanyId alanı zorunludur.",
                StatusCode = 400
            };
        }

        if (dto.ProductId <= 0)
        {
            return new BaseResponseDto
            {
                IsSuccess = false,
                Message = "Geçerli bir ürün seçilmelidir.",
                StatusCode = 400
            };
        }

        if (dto.WarehouseId <= 0)
        {
            return new BaseResponseDto
            {
                IsSuccess = false,
                Message = "Geçerli bir depo seçilmelidir.",
                StatusCode = 400
            };
        }

        if (dto.StorageLocationId <= 0)
        {
            return new BaseResponseDto
            {
                IsSuccess = false,
                Message = "Geçerli bir lokasyon seçilmelidir.",
                StatusCode = 400
            };
        }

        if (dto.Quantity <= 0)
        {
            return new BaseResponseDto
            {
                IsSuccess = false,
                Message = "Miktar sıfırdan büyük olmalıdır.",
                StatusCode = 400
            };
        }

        var product = await _productRepository.GetByIdAsync(dto.ProductId);

        if (product == null)
        {
            return new BaseResponseDto
            {
                IsSuccess = false,
                Message = "Ürün bulunamadı.",
                StatusCode = 404
            };
        }

        if (product.CompanyId != dto.CompanyId)
        {
            return new BaseResponseDto
            {
                IsSuccess = false,
                Message = "Bu ürün için işlem yapma yetkiniz bulunmamaktadır.",
                StatusCode = 403
            };
        }

        var warehouse = await _warehouseRepository.GetByIdAsync(dto.WarehouseId, dto.CompanyId);

        if (warehouse == null)
        {
            return new BaseResponseDto
            {
                IsSuccess = false,
                Message = "Depo bulunamadı.",
                StatusCode = 404
            };
        }

        var location = await _storageLocationRepository.GetByIdAsync(dto.StorageLocationId, dto.WarehouseId, dto.CompanyId);

        if (location == null)
        {
            return new BaseResponseDto
            {
                IsSuccess = false,
                Message = "Lokasyon bulunamadı veya seçilen depoya ait değil.",
                StatusCode = 404
            };
        }

        if (product.CurrentStock < dto.Quantity)
        {
            return new BaseResponseDto
            {
                IsSuccess = false,
                Message = "Yetersiz stok. Çıkış işlemi gerçekleştirilemez.",
                StatusCode = 400
            };
        }

        product.CurrentStock -= dto.Quantity;
        product.UpdatedAt = DateTime.UtcNow;

        var transaction = new StockTransaction
        {
            CompanyId = dto.CompanyId.Trim(),
            ProductId = dto.ProductId,
            WarehouseId = dto.WarehouseId,
            StorageLocationId = dto.StorageLocationId,
            TransactionType = StockTransactionType.Out,
            Quantity = dto.Quantity,
            TransactionDate = DateTime.UtcNow,
            Description = string.IsNullOrWhiteSpace(dto.Description) ? "Stok çıkışı yapıldı." : dto.Description.Trim(),
            CreatedAt = DateTime.UtcNow,
            IsDeleted = false
        };

        await _productRepository.UpdateAsync(product);
        await _stockTransactionRepository.AddAsync(transaction);

        await _productRepository.SaveChangesAsync();

        return new BaseResponseDto
        {
            IsSuccess = true,
            Message = "Stok çıkışı başarıyla yapıldı.",
            StatusCode = 200
        };
    }
}