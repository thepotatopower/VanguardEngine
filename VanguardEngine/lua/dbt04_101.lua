-- 天捌の騎士 コンテニオ

function RegisterAbilities()
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.PutOnGC)
	ability1.SetTrigger("Trigger")
	ability1.SetCondition("Condition")
	ability1.SetActivation("Activation")
end

function Trigger()
	return obj.IsApplicable()
end

function Condition()
	return obj.IsSameZone() and obj.Exists({q.Location, l.PlayerVC, q.NameContains, obj.LoadName("Bastion")})
end

function Activation()
	obj.AddCardValue({q.Other, o.ThisFieldID}, cs.BonusShield, 5000, p.UntilEndOfBattle)
end