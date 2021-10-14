-- Aurora Battle Princess, Kyanite Blue

function NumberOfAbilities()
	return 2
end

function NumberOfParams()
	return 2
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.Deck, q.Other, o.Prison, q.Count, 1
	elseif n == 2 then
		return q.Location, l.PlayerPrisoners, q.Count, 1
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.PlacedOnVC, p.HasPrompt, p.IsMandatory
	elseif n == 2 then
		return a.PlacedOnRC, p.HasPrompt, p.CB, 1, p.SB, 1
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.LastPlacedOnVC() and obj.Exists(1) then
			return true
		end
	elseif n == 2 then
		if obj.LastPlacedOnRC() and obj.Exists(2) then
			return true
		end
	end
	return false
end

function CanFullyResolve(n)
	if n == 1 then
		return true
	elseif n == 2 then
		return true
	end
	return false
end

function Activate(n)
	if n == 1 then
		obj.Search(1)
		obj.Shuffle()
	elseif n == 2 then
		obj.Draw(1)
	end
	return 0
end