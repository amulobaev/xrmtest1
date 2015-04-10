using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using AutoMapper;
using XrmTest1.Core;

namespace XrmTest1.Data
{
    /// <summary>
    /// Реализация репозитария для резюме с использованием Entity Framework Code First
    /// не самый быстрый вариант, с использованием Dapper было бы быстрее
    /// </summary>
    public class ResumeRepository : IRepository<Resume>
    {
        /// <summary>
        /// Статический конструктор
        /// </summary>
        static ResumeRepository()
        {
            // Настройка AutoMapper
            Mapper.CreateMap<Resume, ResumeEntity>()
                .ForMember(dest => dest.ResumeId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Contact.Name));
            Mapper.CreateMap<ResumeEntity, Resume>()
                .BeforeMap((entity, model) => model.Contact = new Contact())
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ResumeId))
                .AfterMap((entity, model) => model.Contact.Name = entity.Name);
        }

        public IEnumerable<Resume> GetAll()
        {
            using (ResumeContext context = new ResumeContext())
            {
                List<ResumeEntity> entities = context.ResumeEntities.ToList();
                List<Resume> models = Mapper.Map<List<ResumeEntity>, List<Resume>>(entities);
                return models;
            }
        }

        public Resume Get(int id)
        {
            using (ResumeContext context = new ResumeContext())
            {
                ResumeEntity entity = context.ResumeEntities.FirstOrDefault(x => x.Id == id);
                return entity != null ? Mapper.Map<ResumeEntity, Resume>(entity) : null;
            }
        }

        public IEnumerable<Resume> GetFiltered(IEnumerable<Filter> filters)
        {
            using (ResumeContext context = new ResumeContext())
            {
                var entities = context.ResumeEntities.AsQueryable();
                
                // Фильтры
                foreach (Filter filter in filters)
                {
                    switch (filter.Type)
                    {
                        case FilterType.Name:
                            string name = (string)filter.Value1;
                            entities = entities.Where(x => x.Name.ToLower().Contains(name.ToLower()));
                            break;
                        case FilterType.Header:
                            string header = (string)filter.Value1;
                            entities = entities.Where(x => x.Header.ToLower().Contains(header.ToLower()));
                            break;
                        case FilterType.Age:
                            try
                            {
                                int ageFrom = (int)filter.Value1;
                                entities = entities.Where(x => x.Age >= ageFrom);
                            }
                            catch
                            {
                                // здесь нечего делать
                            }

                            try
                            {
                                int ageTo = (int)filter.Value2;
                                entities = entities.Where(x => x.Age <= ageTo);
                            }
                            catch (Exception)
                            {
                                // здесь нечего делать
                            }

                            break;
                    }
                }

                // Переводим данные из DTO в модели
                List<Resume> models = Mapper.Map<List<ResumeEntity>, List<Resume>>(entities.ToList());
                return models;
            }
        }

        public void Create(Resume resume)
        {
            if (resume == null)
                throw new ArgumentNullException("resume");

            ResumeEntity entity = Mapper.Map<Resume, ResumeEntity>(resume);
            using (ResumeContext context = new ResumeContext())
            {
                context.ResumeEntities.Add(entity);
                context.SaveChanges();
            }
        }

        public void Update(Resume resume)
        {
            if (resume == null)
                throw new ArgumentNullException("resume");

            using (ResumeContext context = new ResumeContext())
            {
                ResumeEntity entity = context.ResumeEntities.FirstOrDefault(x => x.Id == resume.Id);
                if (entity != null)
                {
                    Mapper.Map(resume, entity);
                    context.SaveChanges();
                }
            }
        }

        public void CreateOrUpdate(Resume resume)
        {
            if (resume == null)
                throw new ArgumentNullException("resume");

            ResumeEntity entity = Mapper.Map<Resume, ResumeEntity>(resume);
            using (ResumeContext context = new ResumeContext())
            {
                context.ResumeEntities.AddOrUpdate(x => x.ResumeId, entity);
                context.SaveChanges();
            }
        }

        public void Delete(Resume resume)
        {
            ResumeEntity entity = Mapper.Map<Resume, ResumeEntity>(resume);
            using (ResumeContext context = new ResumeContext())
            {
                context.ResumeEntities.Remove(entity);
                context.SaveChanges();
            }
        }

    }
}