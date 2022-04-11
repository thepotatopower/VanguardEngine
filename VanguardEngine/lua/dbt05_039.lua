-- 燃える祭師 タマユラ

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
	return obj.PlacedByRidingOn(obj.GetNameFromCardID("dbt05_059"))
end

function Activation()
	obj.Select("Filter", {l.Drop}, 1, 0, {}, -1)
	obj.AddToHand({q.Location, l.Selected})
end

function Filter(id)
	return obj.NameContains(id, obj.LoadName("Ririmi")) or obj.NameContains(id, obj.LoadName("Rarami"))
end