-- 月夜に蕩かして メヒティルト

function RegisterAbilities()
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnAttackHits)
	ability1.SetLocation(l.RC)
	ability1.SetTrigger("Trigger")
	ability1.SetCondition("Condition")
	ability1.SetActivation("Activation")
end

function Trigger()
	return obj.IsAttackingUnit()
end

function Condition()
	return obj.Exists({q.Location, l.PlayerUnits, q.Race, r.Ghost, q.Other, o.NotThisFieldID})
end

function Activation()
	obj.Select({q.Location, l.PlayerUnits, q.Other, o.NotThisFieldID, q.Count, 1})
	obj.AddCardValue({q.Location, l.Selected}, cs.BonusPower, 5000, p.UntilEndOfTurn)
end