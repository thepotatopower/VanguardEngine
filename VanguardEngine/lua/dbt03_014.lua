-- シチームメイジ アシュルダ

function RegisterAbilities()
	-- cont
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.Cont)
	ability1.SetLocation(l.GC)
	ability1.SetActivation("Cont")
	-- on retire from GC
	local ability2 = NewAbility(GetID())
	ability2.SetDescription(2)
	ability2.SetTiming(a.OnRetire)
	ability2.SetMovedFrom(l.GC)
	ability2.SetProperty(p.NotMandatory)
	ability2.SetTrigger("OnRetireTrigger")
	ability2.SetCondition("OnRetireCondition")
	ability2.SetActivation("OnRetire")
end

function Cont()
	if obj.SoulCount() >= 7 then
		obj.AddCardValue({q.Location, l.GC, q.Other, o.This}, cs.BonusShield, 5000, p.Continuous)
	end
end

function OnRetireTrigger()
	return obj.IsApplicable()
end

function OnRetireCondition()
	return obj.CanAddToSoul({q.Other, o.ThisFieldID})
end

function OnRetire()
	if obj.YesNo(obj.GetDescription(3)) then
		obj.AddToSoul({q.Other, o.ThisFieldID})
	end
end