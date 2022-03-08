-- オーセンティックメロディ エスメラルダ

function RegisterAbilities()
	-- on attack
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnAttack)
	ability1.SetLocation(l.RC)
	ability1.SetTrigger("Trigger")
	ability1.SetCondition("Condition")
	ability1.SetActivation("Activation")
end

function Trigger()
	return obj.IsApplicable()
end

function Condition()
	return obj.GetNumberOf("Filter", {l.SungThisTurn}) > 0 and obj.IsSameZone()
end

function Filter(snapshot)
	return snapshot.name == obj.GetNameFromCardID("dlbt01_056")
end

function Activation()
	obj.AddCardValue({q.Other, o.ThisFieldID}, cs.BonusPower, 10000, p.UntilEndOfBattle)
end