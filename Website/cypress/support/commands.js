Cypress.Commands.add('login', (email, password) => {
    cy.visit('/login');
    cy.get('input[name="email"]').type(email);
    cy.get('input[name="password"]').type(password);
    cy.get('.submit-button').click();
    cy.url().should('include', '/profile');
});

Cypress.Commands.add('loginAsAlex', () => {
    cy.login('alexander.davis@example.com', 'Alex1234');
});

Cypress.Commands.add('loginAsMaria', () => {
    cy.login('maria.kovalenko@example.com', 'Maria1234');
});
