-- いつもの放課後 ユーリア

function RegisterAbilities()
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnAttack)
	ability1.SetLocation(l.RC)
	ability1.SetTrigger("Trigger")
	ability1.SetCondition("Condition")
	ability1.SetActivation("Activation")
end

function Trigger()
	return obj.IsBooster() and obj.GetNumberOf("Filter", {l.Units}) > 0
end

function Filter(id)
	return obj.IsPlayer(id) and obj.IsApplicable(id) and obj.HasCardState(id, cs.Friend)
end

function Condition()
	return obj.IsSameZone()
end

function Activation()
	obj.AddCardValue({q.Other, o.ThisFieldID}, cs.BonusPower, 5000, p.UntilEndOfBattle)
end