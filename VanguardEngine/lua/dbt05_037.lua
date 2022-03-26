-- ジプソフィラの妖精 アシェル

function RegisterAbilities()
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.PlacedOnGC)
	ability1.SetTrigger("Trigger")
	ability1.SetCondition("Condition")
	ability1.SetActivation("Activation")
end

function Trigger()
	return obj.IsApplicable()
end

function Condition()
	local count = obj.GetNumOfAttacks({q.Location, l.EnemyUnits, q.Other, o.Attacking})
	return obj.IsSameZone() and count >= 2
end

function Activation()
	obj.AddCardValue({q.Other, o.ThisFieldID}, cs.BonusShield, 15000, p.UntilEndOfBattle)
end