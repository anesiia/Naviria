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
        description: 'invalid name characters',
        user: { ...validUser, name: '@!$' },
        error: "Ім'я введено некоректно",
      },
      {
        description: 'invalid surname characters',
        user: { ...validUser, surname: '123' },
        error: 'Прізвище введено некоректно',
      },
      {
        description: 'invalid email formats',
        emails: ['example.com', 'example@', 'example@com', '@example.com'],
        error: 'Невірний формат пошти',
      },
      {
        description: 'weak password - only letters',
        user: { ...validUser, password: 'Password' },
        passwordCheck: 'Password',
        error: 'Пароль має містити великі та малі літери та цифру',
      },
      {
        description: 'weak password - only numbers',
        user: { ...validUser, password: '12345678' },
        passwordCheck: '12345678',
        error: 'Пароль має містити великі та малі літери та цифру',
      },
      {
        description: 'weak password - lowercase only',
        user: { ...validUser, password: 'abc' },
        passwordCheck: 'abc',
        error: 'Пароль має містити великі та малі літери та цифру',
      },
      {
        description: 'password mismatch',
        user: { ...validUser, password: 'Qwerty123' },
        passwordCheck: 'Pass5678',
        error: 'Паролі не збігаються',
      },
      {
        description: 'no gender selected',
        user: { ...validUser, gender: '' },
        error: 'Оберіть стать',
      },
      {
        description: 'underage user (1 year old)',
        user: { ...validUser, birthDate: `${new Date().getFullYear() - 1}-01-01` },
        error: 'Потрібно бути старше 18 років',
      },
      {
        description: 'nickname with spaces or symbols',
        user: { ...validUser, nickname: 'nickname with space' },
        error: 'Нікнейм має містити лише латинські літери та цифри (3-20 символів)',
      },
      {
        description: 'nickname in non-Latin letters',
        user: { ...validUser, nickname: 'ім’я' },
        error: 'Нікнейм має містити лише латинські літери та цифри (3-20 символів)',
      },
      {
        description: 'nickname too short',
        user: { ...validUser, nickname: 'te' },
        error: 'Нікнейм має містити лише латинські літери та цифри (3-20 символів)',
      },
      {
        description: 'nickname too long (over 20 chars)',
        user: { ...validUser, nickname: '123456789012345678901' },
        error: null, // Якщо помилки не очікується
        customCheck: () => {
          cy.get('input[name="nickname"]')
              .invoke('val')
              .should('have.length', 20);
        }
      },

    ];

    testCases.forEach(({ description, user, passwordCheck, emails, error, customCheck }) => {
      if (emails) {
        emails.forEach((email) => {
          it(`should show error for ${description} - "${email}"`, () => {
            fillForm({ ...validUser, email });
            cy.get('.submit-button').click();
            cy.contains(error).should('be.visible');
          });
        });
      } else if (customCheck) {
        it(`should perform custom check for ${description}`, () => {
          fillForm(user, passwordCheck);
          cy.get('.submit-button').click();
          customCheck();
        });
      } else {
        it(`should show error for ${description}`, () => {
          fillForm(user, passwordCheck);
          cy.get('.submit-button').click();
          cy.contains(error).should('be.visible');
        });
      }
    });
  });
});
