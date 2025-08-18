using Domain.Entity;

namespace Application.WorkspaceCQ.ViewModels
{
    public record WorkspaceViewModel
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public List<ListCard>? Lists { get; set; }
        public Guid? UserId { get; set; }
    }
}
