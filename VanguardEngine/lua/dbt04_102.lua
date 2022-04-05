-- 非業の騎士 デルバイス

function RegisterAbilities()
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.Cont)
	ability1.SetLocation(l.RC)
	ability1.SetActivation("Activation")
end

function Activation()
	local group = obj.GetAbilityTimingCards(a.OnRetire, true, obj.LoadName("Blaster"), true)
	obj.Store(group)
	if obj.GetNumberOf("Filter", {l.RetiredAsCost}) >= 2 then
		obj.AddCardValue({q.Other, o.This}, cs.BonusPower, 10000, p.Continuous)
	end
end

function Filter(snapshot)
	return snapshot.SourceIsAbility() and obj.NameContains(snapshot.abilitySource.name, obj.LoadName("Blaster"))
end