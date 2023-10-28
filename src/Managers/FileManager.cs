using Microsoft.AspNetCore.StaticFiles;

using System.Reflection;

namespace YallaMasar.Managers;

public enum Folder
{
	LocationsCover = 0,
	Locations = 1,
	Places = 2,
	Documents = 3,
}
public class FileResponse
{
	public string ContentType { get; set; }
	public Stream Stream { get; set; }
}

public interface IFileManager
{
	Task<FileResponse> GetWWWRootFileAsync(string folder, string fileName);

	Task<string> SaveFileAsync(Folder folder, IFormFile file, bool keepFileName = false, string folderName = "");
	Task<string> SaveFileAsync(Folder folder, string fileName, string content);
	Task<FileResponse> GeFileAsync(Folder folder, string name, string folderName = "");
	Task DeleteFileAsync(Folder folder, string name);
}

public class FileManager : IFileManager
{
	private readonly string _basePath;
	private readonly IWebHostEnvironment _appEnvironment;

	public FileManager(IConfiguration configuration, IWebHostEnvironment appEnvironment)
	{
		string path = configuration["ServerFilePath"];

		_basePath = Path.Combine(Path.GetDirectoryName(path));
		_appEnvironment = appEnvironment;
	}

	public Task<FileResponse> GeFileAsync(Folder folder, string name, string folderName = "")
	{
		FileResponse result = new();

		string filePath = Path.Combine(_basePath, folder.ToString());

		if (!string.IsNullOrEmpty(folderName))
		{
			filePath = Path.Combine(filePath, folderName);
		}

		filePath = Path.Combine(filePath, name);

		if (File.Exists(filePath))
		{
			FileExtensionContentTypeProvider provider = new FileExtensionContentTypeProvider();

			Stream stream = new StreamReader(filePath).BaseStream;

			if (!provider.TryGetContentType(filePath, out string contentType))
			{
				contentType = "application/octet-stream";
			}

			result = new FileResponse { Stream = stream, ContentType = contentType };
		}
		else
		{
			Console.WriteLine($"Couldnt file image at the path: {filePath}");
		}

		return Task.FromResult(result);
	}

	public async Task<string> SaveFileAsync(Folder folder, IFormFile file, bool keepFileName = false, string folderName = "")
	{
		if (file == null || file.Length == 0)
		{
			throw new Exception("InvalidFile");
		}

		string fileName = keepFileName ? file.FileName : $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";

		string pathToSave = Path.Combine(_basePath, folder.ToString());

		if (!string.IsNullOrEmpty(folderName))
		{
			pathToSave = Path.Combine(pathToSave, folderName);
		}

		if (!Directory.Exists(pathToSave))
		{
			Directory.CreateDirectory(pathToSave);
		}

		string filePath = Path.Combine(pathToSave, fileName);

		using FileStream stream = File.Create(filePath);

		await file.CopyToAsync(stream);

		return fileName;
	}

	public Task<FileResponse> GetWWWRootFileAsync(string folder, string fileName)
	{
		FileResponse result = new();

		var filePath = Path.Combine(_appEnvironment.WebRootPath, folder.ToString());

		filePath = Path.Combine(filePath, fileName);

		if (File.Exists(filePath))
		{
			var provider = new FileExtensionContentTypeProvider();

			var stream = new StreamReader(filePath).BaseStream;

			if (!provider.TryGetContentType(filePath, out string contentType))
			{
				contentType = "application/octet-stream";
			}

			result = new FileResponse { Stream = stream, ContentType = contentType };
		}

		return Task.FromResult(result);
	}

	public async Task<string> SaveFileAsync(Folder folder, string fileName, string content)
	{
		if (string.IsNullOrEmpty(content))
		{
			throw new Exception("InvalidFile");
		}

		string pathToSave = Path.Combine(_basePath, folder.ToString());

		if (!Directory.Exists(pathToSave))
		{
			Directory.CreateDirectory(pathToSave);
		}

		string filePath = Path.Combine(pathToSave, fileName);

		using StreamWriter file = new(filePath);

		await file.WriteLineAsync(content);

		return fileName;
	}

	public Task DeleteFileAsync(Folder folder, string name)
	{
		string filePath = Path.Combine(_basePath, folder.ToString(), name);

		if (File.Exists(filePath))
		{
			File.Delete(filePath);
		}

		return Task.CompletedTask;
	}
}