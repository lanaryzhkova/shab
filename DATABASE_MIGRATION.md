# �������� ���� ������ ��� ������ �����������

## �������� ��������

��������� ��������� ������� � �������� ������� `RESTful.Infrastructure`:

```bash
# �������� ��������
dotnet ef migrations add InitialConferenceModule --startup-project ..\RESTful.API\RESTful.API.csproj

# ��������� �������� � ���� ������
dotnet ef database update --startup-project ..\RESTful.API\RESTful.API.csproj
```

## ��������� ���� ������

### ������� Participants (���������)
- **Id** (int, PK) - ������������� ���������
- **FirstName** (nvarchar(100)) - ���
- **LastName** (nvarchar(100)) - �������
- **MiddleName** (nvarchar(100)) - ��������
- **WorkPlace** (nvarchar(200)) - ����� ������
- **AcademicDegree** (nvarchar(100), nullable) - ������ �������
- **AcademicTitle** (nvarchar(100), nullable) - ������ ������
- **Email** (nvarchar(100), unique) - Email
- **Phone** (nvarchar(20)) - �������
- **Role** (int) - ���� (1=���������, 2=���������)
- **RegistrationDate** (datetime2) - ���� �����������

### ������� Presentations (�������)
- **Id** (int, PK) - ������������� �������
- **Title** (nvarchar(300)) - �������� �������
- **Abstract** (nvarchar(2000), nullable) - ���������
- **MainSpeakerId** (int, FK) - ID ��������� ����������
- **CreatedDate** (datetime2) - ���� ��������

### ������� CoSpeakers (������������)
- **Id** (int, PK) - ������������� ������
- **PresentationId** (int, FK) - ID �������
- **ParticipantId** (int, FK) - ID ���������-������������
- **CanEdit** (bit) - ����� �� ��������������
- **AddedDate** (datetime2) - ���� ����������

### �����
- Participant ? Presentations (1:N) - ���� �������� ����� ���� �������� ����������� ���������� ��������
- Presentation ? CoSpeakers (1:N) - � ������ ������� ����� ���� ��������� �������������
- Participant ? CoSpeakers (1:N) - ���� �������� ����� ���� ������������� � ���������� ��������

### �������
- UNIQUE �� Participants.Email
- UNIQUE �� CoSpeakers (PresentationId, ParticipantId) - ������������� ������������ �������������
