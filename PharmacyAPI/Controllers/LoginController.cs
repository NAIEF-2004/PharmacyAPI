using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PharmacyAPI.Controllers
{
    [Route("api/auth")] // تحديد المسار الأساسي لهذا الـ Controller.
    [ApiController] // تحديد أن هذا الـ Controller خاص بالـ API ويتضمن ميزات مثل التحقق التلقائي من الطلب.
    [ApiVersion("1.0")] 
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration _config; // حقن إعدادات التكوين (Configuration) للوصول إلى الإعدادات المخزنة.

        public LoginController(IConfiguration config)
        {
            _config = config; // حفظ إعدادات التكوين في متغير محلي.
        }

        [HttpPost("login")] // يحدد أن هذا الإجراء يستخدم بروتوكول HTTP POST وعنوانه الإضافي هو "login".
        public IActionResult Login([FromBody] LoginRequest request) // الإجراء المسؤول عن معالجة طلب تسجيل الدخول.
        {
            // التحقق من صحة بيانات المستخدم (اسم المستخدم وكلمة المرور).
            if (request.Username == "admin" && request.Password == "password123")
            {
                var token = GenerateToken(); // توليد رمز (JWT) إذا كانت البيانات صحيحة
                return Ok(new { Token = token }); // إعادة الرمز إلى المستخدم مع حالة 200 
            }
            return Unauthorized(); // إذا كانت البيانات خاطئة، يتم إرجاع حالة 401
        }

        private string GenerateToken() // دالة لتوليد رمز JSON Web Token (JWT)
        {
            // استخراج المفتاح السري المخزن في الإعدادات.
            var key = Encoding.UTF8.GetBytes(_config["JwtSettings:Key"]);

            var tokenHandler = new JwtSecurityTokenHandler(); // كائن او انستانس للتعامل مع إنشاء الـ JWT.

            // وصف لتكوين الرمز 
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                // تحديد بيانات المستخدم كجزء من الرمز (Claim)
                Subject = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, "admin") }),

                // تحديد وقت انتهاء صلاحية الرمز (ساعة واحدة من الآن)
                Expires = DateTime.UtcNow.AddHours(1),

                // تحديد طريقة التوقيع الرقمي باستخدام المفتاح السري
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256),

                // تحديد الناشر (Issuer) والمستقبل (Audience) للرمز
                Issuer = _config["JwtSettings:Issuer"],
                Audience = _config["JwtSettings:Audience"]
            };

            // إنشاء الرمز باستخدام التكوين السابق
            var token = tokenHandler.CreateToken(tokenDescriptor);

            // إرجاع الرمز في شكل نصي.
            return tokenHandler.WriteToken(token);
        }
    }

    // نموذج بيانات طلب تسجيل الدخول (Login Request)
    public class LoginRequest
    {
        public string Username { get; set; } // اسم المستخدم
        public string Password { get; set; } // كلمة المرور
    }
}
