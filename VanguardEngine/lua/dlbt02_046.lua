-- 心弾む指先 エデルガルト

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
	return obj.IsApplicable()
end

function Condition()
	return obj.IsSameZone() and obj.Exists({q.Location, l.PlayerRC, q.Race, r.Ghost, q.Other, o.NotThisFieldID})
end

function Activation()
	local power = 2000 * obj.GetNumberOf({q.Location, l.PlayerRC, q.Race, r.Ghost, q.Other, o.NotThisFieldID})
	obj.AddCardValue({q.Other, o.ThisFieldID}, cs.BonusPower, power, p.UntilEndOfBattle)
end