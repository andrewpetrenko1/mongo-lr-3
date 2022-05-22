using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using mongo_lr3.Models;
using mongo_lr3.Repositories.Interfaces;
using mongo_lr3.Services.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;

namespace mongo_lr3.Controllers
{
    public class MemberController : Controller
    {
        private readonly IMemberRepository _repository;
        private readonly IFileService _fService;
        public MemberController(IMemberRepository memberRepository, IFileService fileService)
        {
            _repository = memberRepository ?? throw new ArgumentNullException(nameof(memberRepository));
            _fService = fileService ?? throw new ArgumentNullException(nameof(fileService));
        }
        public async Task<ActionResult> Index(RequestParameters parameters)
        {
            List<Member> members = new List<Member>();
            var filter = new FilterDefinitionBuilder<Member>().Empty;
            if (parameters.Age == 0 && parameters.Name == null)
                return View(await _repository.GetAll());

            if(parameters.Name != null)
                filter = Builders<Member>.Filter.Regex(m => m.Name, new BsonRegularExpression(parameters.Name, "i"));
            if (parameters.Age > 0)
                filter &= Builders<Member>.Filter.Eq(m => m.Age, parameters.Age);
            members = (List<Member>)await _repository.GetList(filter);

            return View(members);
        }

        public async Task<ActionResult> Get(string id)
        {
            var member = await _repository.Get(el => el.Id, ObjectId.Parse(id));
            return Ok(member);
        }

        public async Task<ActionResult> Details(string id)
        {
            var member = await _repository.Get(el => el.Id, ObjectId.Parse(id));
            return View(member);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Member member, IFormFile file)
        {
            if (file != null)
            {
                using (var reader = new StreamReader(file.OpenReadStream()))
                {
                    var stream = reader.BaseStream;
                    member.ImageId = (await _fService.StoreImage(member, stream, file.FileName)).ImageId;
                }
            }
            await _repository.Insert(member);
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Member member, IFormCollection formColl, IFormFile file)
        {
            if (file != null)
            {
                using (var reader = new StreamReader(file.OpenReadStream()))
                {
                    var stream = reader.BaseStream;
                    member.ImageId = (await _fService.StoreImage(member, stream, file.FileName)).ImageId;
                }
            }
            member.Id = ObjectId.Parse(formColl["id"][0]);
            await _repository.Update(m => m.Id, member.Id, member);
            return RedirectToAction("Index");
        }

        // GET: HomeController/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            await _repository.Delete(m => m.Id, ObjectId.Parse(id));
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> GetImage(string id)
        {
            var image = await _fService.GetImage(id);
            if(image == null) return null;

            return File(image, "image/png");
        }
    }
}
