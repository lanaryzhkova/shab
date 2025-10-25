# ������ ����������� �� �����������

## ��������

������ ������������� ���������������� ��� ����������� ���������� �� ����������� � ���� �����:
- **���������** - �������� ����������� ��� �������
- **���������** - �������� � ��������

## �������� �����������

### 1. ����������� ����������
- ����������� � ������ ����������� (���, ����� ������, �������, ������, ��������)
- ����� ����: ��������� ��� ���������
- ��� ����������� - ���������� � ������� (��������, ���������)
- ����������� ���������� �������������

### 2. ���������� ���������
- �������� ��������� ����� ������ ������ � �������
- ������������ ����� ������������� ���������� � �������
- ����������� ��������� ���� ������� ��� ������������� (CanEdit)

### 3. �������� ����������
- ������ ���� ���������� � ����������� �� ����
- ��������� ���������� �� ���������
- ������ �������� �����������
- �������� �������� � ������ ���� �������

## ����������� �������

### ������� �������

1. **RESTful.Domain** - �������� ������ � ����������
   - `Entities/` - �������� �� (Participant, Presentation, CoSpeaker)
   - `DTOs/` - DTO ��� �������� ������
   - `Interfaces/` - ���������� ������������

2. **RESTful.Infrastructure** - ���������������� ����
   - `Data/Context.cs` - �������� Entity Framework
   - `Repositories/` - ���������� ������������

3. **RESTful.API** - RESTful API
   - `Controllers/ParticipantsController.cs` - API ����������
   - `Controllers/PresentationsController.cs` - API ��������

4. **Frontend** - Blazor ���������
   - `Services/ConferenceService.cs` - ������ ��� ������ � API
   - `Components/Pages/` - �������� ����������

## API Endpoints

### ���������

#### POST /api/participants/register
����������� ������ ���������

**Request Body:**
```json
{
  "firstName": "����",
  "lastName": "������",
  "middleName": "��������",
  "workPlace": "���",
  "academicDegree": "�.�.�.",
  "academicTitle": "������",
  "email": "ivanov@example.com",
  "phone": "+79991234567",
  "role": 2,
  "presentation": {
    "title": "������������ ������� ��������� ��������",
    "abstract": "������ ��������...",
"coSpeakerIds": [1, 2]
  }
}
```

**Response:**
```json
{
  "id": 1,
  "firstName": "����",
  "lastName": "������",
  "middleName": "��������",
  "fullName": "������ ���� ��������",
  "workPlace": "���",
  "academicDegree": "�.�.�.",
  "academicTitle": "������",
  "email": "ivanov@example.com",
  "phone": "+79991234567",
  "role": 2,
  "registrationDate": "2024-01-15T10:30:00Z",
  "presentations": [...]
}
```

#### GET /api/participants
�������� ���� ����������

#### GET /api/participants/{id}
�������� ��������� �� ID

#### GET /api/participants/{id}/presentations
�������� ������� ��������� (��� ��������� ���������� � ������������)

### �������

#### GET /api/presentations
�������� ��� �������

#### GET /api/presentations/{id}?participantId={participantId}
�������� ������ �� ID � ������ ���� ������� ��� ���������� ���������

## Blazor ��������

### /conference/register
�������� ����������� �� �����������
- ����� � ����������
- ������������ ����������� ����� ��� �����������
- ����� ������������� �� ��� ������������������ ����������

### /conference/participants
������ ���� ����������
- ������� � �����������
- ���������� �� ����
- ������ �� ��������� ����������

### /conference/participants/{id}
��������� ���������� �� ���������
- ������ ���������� �� ���������
- ������ �������� (��� ��������� ���������� � ������������)
- ��������� ���� ������� � ��������

### /conference/presentations
������ ���� �������� �����������
- �������� ��������
- ���������� � �����������

## ��������� �������

### 1. ��������� ���� ������

�������� `RESTful.API\appsettings.json` � ��������� ������ �����������:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=ConferenceDB;Trusted_Connection=True;MultipleActiveResultSets=true"
  }
}
```

### 2. �������� ��������

��������� ������� �� ��������� � �������� `RESTful.Infrastructure`:

```bash
dotnet ef migrations add InitialConferenceModule --startup-project ..\RESTful.API\RESTful.API.csproj
dotnet ef database update --startup-project ..\RESTful.API\RESTful.API.csproj
```

### 3. ��������� URL API

� ����� `Frontend\appsettings.json` ������� URL ������ API:

```json
{
  "ApiBaseUrl": "https://localhost:7001/"
}
```

### 4. ��������� CORS

� ����� `RESTful.API\Program.cs` ��������� CORS ��� ������ Blazor ����������. �� ��������� ��������� ����� 7149 � 5149.

### 5. ������ ��������

��������� ��� �������:
- **RESTful.API** - API ������ (������ https://localhost:7001)
- **Frontend** - Blazor ���������� (������ https://localhost:7149)

## �������� ������

### Participant (��������)
```csharp
public class Participant
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string MiddleName { get; set; }
    public string WorkPlace { get; set; }
    public string? AcademicDegree { get; set; }
    public string? AcademicTitle { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public ParticipantRole Role { get; set; }
    public DateTime RegistrationDate { get; set; }
}

public enum ParticipantRole
{
    Listener = 1,
    Speaker = 2
}
```

### Presentation (������)
```csharp
public class Presentation
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string? Abstract { get; set; }
    public int MainSpeakerId { get; set; }
    public DateTime CreatedDate { get; set; }
    public Participant MainSpeaker { get; set; }
  public ICollection<CoSpeaker> CoSpeakers { get; set; }
}
```

### CoSpeaker (�����������)
```csharp
public class CoSpeaker
{
    public int Id { get; set; }
    public int PresentationId { get; set; }
    public int ParticipantId { get; set; }
    public bool CanEdit { get; set; }
    public DateTime AddedDate { get; set; }
    public Presentation Presentation { get; set; }
    public Participant Participant { get; set; }
}
```

## ����� ������� � ��������

### �������� ���������
- ������ ������ � �������
- ����� ������������� ��� ����������

### ������������
- ����� ������ �� ��������� (CanEdit = false)
- ����� ������������� ��������, ���������, ������ ����������
- ���� CanEdit = true - ����� ������������� ������

### ����������� ���������
- �� ����� ������� � ��������� ���������� � �������
- ����� ������ ������ � ����� ������

## ���������

��� ����� ����� ��������� �� ������� ������� � �������:
- ������������ ���� �������� (*)
- Email ����������� �� ������������ �������
- ������� ����������� �� ������������ �������
- ������������ email � ���� ������

## ����������� ����������

1. **����� ��������� ��� �������������** - ������������ ����� ������ ������������� ������, ���� �� ���������� ���� CanEdit
2. **������������ email** - ���� email = ���� ��������
3. **��������� ��������** - ��� �������� ������� ��������� ��� ����� � ��������������
4. **������ �� ������������** - ���� �������� �� ����� ���� �������� ������������� ������ � ������ �������

## ��������� ����������

1. ���������� �������������� � �����������
2. �������������� ���������� �� ���������
3. �������������� �������� �������� �����������
4. ���������� ������� �������������
5. �������� ������ (�����������, ���������)
6. ���������� �����������
7. ����������� �� ������
8. Email-�����������
9. ������� ������� ���������� � ��������
10. ����� �� ���������� � ��������

## ����������

- .NET 8
- ASP.NET Core Web API
- Blazor Server
- Entity Framework Core 9
- SQL Server (LocalDB)
- Bootstrap 5

## ��������� ������

```
WebApplication1/
??? RESTful.Domain/
?   ??? Entities/
?   ?   ??? Participant.cs
?   ?   ??? Presentation.cs
?   ?   ??? CoSpeaker.cs
?   ??? DTOs/
?   ?   ??? ConferenceDto.cs
?   ??? Interfaces/
?       ??? IRepositories.cs
??? RESTful.Infrastructure/
?   ??? Data/
?   ?   ??? Context.cs
???? Repositories/
?       ??? ParticipantRepository.cs
?    ??? PresentationRepository.cs
?       ??? CoSpeakerRepository.cs
??? RESTful.API/
?   ??? Controllers/
?   ?   ??? ParticipantsController.cs
?   ?   ??? PresentationsController.cs
?   ??? Program.cs
?   ??? appsettings.json
??? Frontend/
??? Components/
    ?   ??? Pages/
    ?   ?   ??? ConferenceRegistration.razor
    ?   ?   ??? ParticipantsList.razor
    ?   ?   ??? ParticipantDetail.razor
    ?   ?   ??? PresentationsList.razor
    ?   ??? Layout/
    ? ??? NavMenu.razor
 ??? Services/
    ?   ??? ConferenceService.cs
    ??? Program.cs
    ??? appsettings.json
```
