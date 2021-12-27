-- 決意の精霊王 オルバリア

function RegisterAbilities()
	-- over trigger
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnOverTrigger)
	ability1.SetTrigger("Trigger")
	ability1.SetActivation("Activation")
end

function Trigger()
	return obj.IsApplicable()
end

function Activation()
	obj.Select({q.Location, l.PlayerUnits, q.Count, 1})
	obj.AddCardValue({q.Location, l.Selected}, cs.BonusPower, 100000000, p.UntilEndOfTurn)
end