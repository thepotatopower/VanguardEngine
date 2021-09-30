-- The Sound of Waves at Twilight

function NumberOfAbilities()
	return 3
end

function NumberOfParams()
	return 2
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.PlayerVC, q.Count, 1
	elseif n == 2 then
		return q.Location, l.FrontRow
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.OnOrder, p.HasPrompt
	elseif n == 2 then
		return a.PutOnOrderZone, p.HasPrompt
	elseif n == 3 then
		return a.OnSing, p.HasPrompt
	end
end

function CheckCondition(n)
	if n == 1 then
		return true
	elseif n == 2 then
		if obj.LastPutOnOrderZone() then
			return true
		end
	elseif n == 3 then
		if obj.IsInOrderZone() then
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
	elseif n == 3 then
		return true
	end
	return false
end

function Activate(n)
	if n == 2 then
		obj.ChooseAddTempPower(1, 5000)
	elseif n == 3 then
		obj.AddTempPower(2, 5000)
	end
	return 0
end