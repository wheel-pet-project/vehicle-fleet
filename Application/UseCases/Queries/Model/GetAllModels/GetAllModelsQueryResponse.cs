namespace Application.UseCases.Queries.Model.GetAllModels;

public record GetAllModelsQueryResponse
{
    private readonly List<ModelShortView> _models;

    public GetAllModelsQueryResponse(List<ModelShortView> models)
    {
        _models = models;
    }

    public IReadOnlyList<ModelShortView> Models => _models.AsReadOnly();

    public record ModelShortView(Guid Id, string Brand, string CarModel);
}