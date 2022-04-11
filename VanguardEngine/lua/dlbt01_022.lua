-- Six-Flower Fractale

function NumberOfAbilities()
	return 3
end

function NumberOfParams()
	return 4
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.PlayerOrder, q.Other, o.Song, q.Other, o.FaceDown, q.Count, 1, q.Min, 0
	elseif n == 2 then
		return q.Location, l.PlayerOrder, q.Other, o.Song, q.Other, o.FaceDown
	elseif n == 3 then
		return q.Location, l.PlayerRC, q.Count, 0
	elseif n == 4 then
		return q.Location, l.PlayerVC, q.Count, 1
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
		if obj.LastPutOnOrderZone() and obj.Exists(1) then
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
		if obj.Exists(1) then
			return true
		end
	end
	return false
end

function Activate(n)
	if n == 2 then
		obj.ChooseFlipFaceUp(1)
	elseif n == 3 then
		obj.Inject(3, q.Count, obj.GetNumberOf(2))
		obj.Store(obj.ChooseStand(3))
		obj.ChooseAddTempPower(4, obj.GetNumberOf(obj.Stored()) * 10000)
	end
	return 0
end