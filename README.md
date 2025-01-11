# Cloud Middle Service
# Môi trường phát triển

Yêu cầu:
Visual Studio 2022 17.*.* với .Net 9
https://visualstudio.microsoft.com/vs/community/
hoặc cài bổ sung .net 9 https://dotnet.microsoft.com/en-us/download/dotnet/9.0 nếu đã có Visual Studio

# Môi trường hosting
1. Với Windows cần IIS
2. Cài đặt .net 9 cho hosting https://dotnet.microsoft.com/en-us/download/dotnet/thank-you/runtime-aspnetcore-9.0.0-windows-hosting-bundle-installer
3. Tạo site cho WebApi
4. Cầu hình appPool là Aways running
5. Kiểm tra service : http://localhost/Alive
6. Sử dụng gửi dữ liệu với Post:
   Cầu trúc Body:
   {
      ApiMethod="",
      Body={}
   }
8. Sử dụng kéo dữ liệu với get:
   Cấu trúc http://localhost/Connect?query= apiName
