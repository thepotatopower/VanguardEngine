-- Aurora Battle Princess, Derii Violet

function NumberOfAbilities()
	return 1
end

function NumberOfParams()
	return 4
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.PlayerPrisoners, q.Count, 1
	elseif n == 2 then
		return q.Location, l.Soul, q.Count, 1
	elseif n == 3 then
		return q.Location, l.PlayerRC, q.Location, l.PlayerVC, q.Count, 1
	elseif n == 4 then
		return q.Location, l.Selected
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.PutOnGC, t.Auto, p.HasPrompt, true, p.IsMandatory, false, p.SB, 2
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.LastPlacedOnGC() and obj.Exists(1) and obj.CanSB(2) then
			return true
		end
	end
	return false
end

function CanFullyResolve(n)
	if n == 1 then
		return true
	end
	return false
end

function Cost(n)
	if n == 1 then
		obj.SoulBlast(2)
	end
end

function Activate(n)
	if n == 1 then
		obj.Select(3)
		obj.HitImmunity(4, 0, 1, 2)
		obj.EndSelect()
	end
	return 0
end