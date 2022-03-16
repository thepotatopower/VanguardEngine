-- 縁由の忍鬼 ツムギ

function RegisterAbilities()
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.PlacedOnRC)
	ability1.SetTrigger("Trigger")
	ability1.SetCost("Cost")
	ability1.SetCanFullyResolve("CanFullyResolve")
	ability1.SetActivation("Activation")
end

function Trigger()
	return obj.IsApplicable()
end

function Cost(check)
	if check then return obj.CanSB(1) end
	obj.SoulBlast(1)
end

function CanFullyResolve()
	return obj.GetNumberOf("Filter", {l.Drop}) > 0
end

function Filter(id) 
	return obj.IsUnitType(id, u.Normal) and (obj.NameContains(id, "Stealth") or obj.HasProperty(id, p.IsGlitter) or obj.HasProperty(id, p.Glitter))
end

function Activation()
	obj.Select("Filter", {l.Drop}, 1, 1, {}, -1)
	obj.AddToSoul({q.Location, l.Selected})
end
