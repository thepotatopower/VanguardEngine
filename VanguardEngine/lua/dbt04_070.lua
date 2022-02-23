-- 焔の拳僧 チョウキ

function RegisterAbilities()
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.PutOnGC)
	ability1.SetTrigger("Trigger")
	ability1.SetCost("Cost")
	ability1.SetCanFullyResolve("CanFullyResolve")
	ability1.SetActivation("Activation")
end

function Trigger()
	return obj.IsApplicable()
end

function Cost(check)
	if check then return obj.CanReveal({q.Location, l.Hand, q.Grade, 4, q.Count, 1}) end
	obj.ChooseReveal({q.Location, l.Hand, q.Grade, 4, q.Count, 1})
end

function CanFullyResolve()
	return obj.IsSameZone()
end

function Activation()
	obj.AddCardValue({q.Other, o.ThisFieldID}, cs.BonusShield, 10000, p.UntilEndOfBattle)
end