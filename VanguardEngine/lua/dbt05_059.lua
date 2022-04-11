-- 穏やかな日差しの中で タマユラ

function RegisterAbilities()
	-- placed on vc
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.PlacedOnVC)
	ability1.SetTrigger("Trigger")
	ability1.SetActivation("Activation")
	-- glitter
	local ability2 = NewAbility(GetID())
	ability2.SetDescription(2)
	ability2.SetProperty(p.IsGlitter)
end

function Trigger()
	return obj.PlacedByRidingOn(obj.GetNameFromCardID("dbt05_061"))
end

function Activation()
	obj.LookAtTopOfDeck(7)
	obj.Search("Filter", {l.Looking}, 1, 0, {})
end

function Filter(id)
	return obj.NameContains(id, obj.LoadName("Ririmi")) or obj.NameContains(id, obj.LoadName("Rarami")) or obj.IsOrderType(id, ot.FoxArt)
end