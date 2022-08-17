-- 天砕く激情

function RegisterAbilities()
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnOrder)
	ability1.SetCondition("Condition")
	ability1.SetCost("Cost")
	ability1.SetCanFullyResolve("CanFullyResolve")
	ability1.SetActivation("Activation")
end

function Condition()
	return obj.GetNumberOf("Filter", {l.PlayerVC}) > 0
end

function Filter(id)
	return obj.HasProperty(id, p.RevolDress)
end

function Cost(check)
	if check then return obj.CanCB(1) end
	obj.CounterBlast(1)
end

function CanFullyResolve()
	return obj.CanAddToHand({q.Location, l.Deck, q.Name, obj.LoadNameFromCardID("dtd03_005")})
end

function Activation()
	obj.Search({q.Location, l.Deck, q.Name, obj.LoadNameFromCardID("dtd03_005"), q.Count, 1, q.Min, 0})
end