using EmailSender.Config;
using EmailSender.Data;
using EmailSender.PipelineHandlers;
using EmailSender.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace EmailSender
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDataAccess();
            services.AddPipelineHandlers();

            services.AddControllers(c =>
            {
                c.Filters.Add(typeof(MailSendApiExceptionHandlerFilter), -9999);
            });

            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IEmailValidationService, EmailValidationService>();

            services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo {Title = "EmailSender.Web", Version = "v1"}); });

            #region Jobs

            services.Configure<TasksPollTriggerOptions>(Configuration.GetSection(TasksPollTriggerOptions.Key));
            services.Configure<CheckRecipientServiceOptions>(Configuration.GetSection(CheckRecipientServiceOptions.Key));
            services.Configure<EmailTypeRouterServiceOptions>(Configuration.GetSection(EmailTypeRouterServiceOptions.Key));
            services.Configure<PriorityScoreFilterServiceOptions>(Configuration.GetSection(PriorityScoreFilterServiceOptions.Key));
            services.Configure<PriorityScoreRouterServiceOptions>(Configuration.GetSection(PriorityScoreRouterServiceOptions.Key));
            services.Configure<RegularEmailSenderServiceOptions>(Configuration.GetSection(RegularEmailSenderServiceOptions.Key));
            services.Configure<EmailRejectionServiceOptions>(Configuration.GetSection(EmailRejectionServiceOptions.Key));

            services.AddHostedService<PipelineTriggerService>();
            services.AddHostedService<CheckRecipientService>();
            services.AddHostedService<EmailTypeRouterService>();
            services.AddHostedService<PriorityScoreFilterService>();
            services.AddHostedService<PriorityScoreRouterService>();
            services.AddHostedService<RegularEmailSenderService>();
            services.AddHostedService<EmailRejectionService>();

            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "EmailSender.Web v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}
