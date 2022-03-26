-- オーギュラス・ライオン

function RegisterAbilities()
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.PlacedOnRC)
	ability1.SetTrigger("Trigger")
	ability1.SetCondition("Condition")
	ability1.SetCost("Cost")
	ability1.SetCanFullyResolve("CanFullyResolve")
	ability1.SetActivation("Activation")
end

function Trigger()
	return obj.IsApplicable() and not obj.SourceNameIs(obj.GetNameFromCardID("dbt05_080"))
end

function Condition()
	return obj.PersonaRode()
end

function Cost(check)
	if check then return obj.CanCB(1) end
	obj.CounterBlast(1)
end

function CanFullyResolve()
	return obj.CanSuperiorCall({q.Location, l.Deck, q.Name, obj.GetNameFromCardID("dbt05_080"), q.Count, 1})
end

function Activation()
	obj.SuperiorCall({"Filter"}, {l.Deck}, 1, 0, "", {p.AsRest}, {})
	obj.Shuffle()
end

function Filter(id)
	return obj.IsName(id, obj.GetNameFromCardID("dbt05_080"))
end