describe('Registration Page - Negative Cases Only', () => {

  const validUser = {
    name: 'Лайт',
    surname: 'Ягамі',
    email: 'light.yagami@example.com',
    password: 'Qwerty123',
    nickname: 'Kira3000',
    gender: 'm',
    birthDate: '2000-01-01',
  };

  beforeEach(() => {
    cy.visit('/registration');
  });

  const fillForm = (user = validUser, passwordCheck = user.password) => {
    cy.get('input[name="name"]').clear().type(user.name);
    cy.get('input[name="surname"]').clear().type(user.surname);
    cy.get('input[name="email"]').clear().type(user.email);
    cy.get('input[name="password"]').clear().type(user.password);
    cy.get('input[name="repeat-password"]').clear().type(passwordCheck);

    if (user.gender) {
      cy.get(`input[type="radio"][value="${user.gender}"]`).check({ force: true });
    }

    if (user.birthDate) {
      cy.get('input[name="birth-date"]').clear().type(user.birthDate);
    } else {
      cy.get('input[name="birth-date"]').clear();
    }

    cy.get('input[name="nickname"]').clear().type(user.nickname);
  };



  describe('Validation Errors', () => {
    const testCases = [
      {
        description: 'invalid name',
        user: { ...validUser, name: '1234' },
        error: "Ім'я введено некоректно",
      },
      {
        description: 'invalid surname',
        user: { ...validUser, surname: '!!!' },
        error: 'Прізвище введено некоректно',
      },
      {
        description: 'invalid email',
        user: { ...validUser, email: 'invalid_email' },
        error: 'Невірний формат пошти',
      },
      {
        description: 'weak password',
        user: { ...validUser, password: '123' },
        passwordCheck: '123',
        error: 'Пароль має містити великі та малі літери та цифру',
      },
      {
        description: 'mismatched passwords',
        user: { ...validUser, password: 'Qwerty123' },
        passwordCheck: 'Another123',
        error: 'Паролі не збігаються',
      },
      {
        description: 'missing gender',
        user: { ...validUser, gender: '' },
        error: 'Оберіть стать',
      },
      {
        description: 'missing birth date',
        user: { ...validUser, birthDate: '' },
        error: 'Вкажіть дату народження',
      },
      {
        description: 'underage user',
        user: {
          ...validUser,
          birthDate: `${new Date().getFullYear() - 17}-01-01`,
        },
        error: 'Потрібно бути старше 18 років',
      },
      {
        description: 'invalid nickname',
        user: { ...validUser, nickname: '!!!!' },
        error: 'Нікнейм має містити лише латинські літери та цифри',
      },
      {
        description: 'name longer than 20 characters',
        user: { ...validUser, nickname: 'Су' },
        error: "Нікнейм має містити лише латинські літери та цифри (3-20 символів)",
      }

    ];

    testCases.forEach(({ description, user, error, passwordCheck }) => {
      it(`should show error for ${description}`, () => {
        fillForm(user, passwordCheck);
        cy.get('.submit-button').click();
        cy.contains(error).should('be.visible');
      });
    });
  });
});
